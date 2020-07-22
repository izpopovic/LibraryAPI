using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LibraryAPI.Models;
using LibraryAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MRZScanController : ControllerBase
    {
        private ApiDbContext _apiDbContext;

        #region Constructors
        public MRZScanController(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        #endregion

        // Set the filepath for a picture of an document you want to be scanned
        // All of these can be put in configuration (appSettings.json)
        private const string imageFilepath = @"C:\Users\Programmer\Desktop\osobna3.jpg";

        private const string bearerValue = "MmM0Y2RmOGEzNmQ4NDMyYWI1YzA2M2E5YWU5NjM4ZTg6MzZmMWRhNTMtZjE3MC00OWY0LTllOTMtY2IwOGM2ZGUxNDRl";
        private const string fullUrl = "https://api.microblink.com/recognize/execute/?api_key=2c4cdf8a36d8432ab5c063a9ae9638e8";

        // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient Client = new HttpClient();

        // GET: api/<MRZScanController>
        [HttpGet]
        public async Task<string> Get()
        {
            bool isValid = false;

            string image64base = ParseImageToBase64(imageFilepath);
            if (!image64base.IsNullOrEmpty())
            {
                var requestReturnValue = await CreateRequestAsync(fullUrl, image64base);
                if (!requestReturnValue.IsNullOrEmpty())
                {
                    var root = JObject.Parse(requestReturnValue);
                    var rawMRZString = root["data"]["result"]["rawMRZString"].ToString();

                    var fullDataArray = rawMRZString.Split('\n');

                    #region CHECK DIGITS

                    #region DocumentNumber
                    // Take documents number from upper line
                    var documentNumber = fullDataArray[0].Skip(5).Take(9).ToArray();
                    // Take document check digit from upper line
                    var documentNumberCheckChar = fullDataArray[0].Skip(14).Take(1).ToArray();

                    int documentNumberCheckDigit;
                    bool correctDocumentNumberCheckDigit = false;
                    if (int.TryParse(documentNumberCheckChar, out documentNumberCheckDigit))
                    {
                        correctDocumentNumberCheckDigit = MRZScanUtils.CheckDigit(documentNumber, documentNumberCheckDigit);
                    }
                    else
                    {
                        Console.WriteLine("Parsing of check digit for document number failed!");
                    }
                    #endregion

                    #region DateOfBirth

                    // Take date of birth from middle line
                    var dateOfBirth = fullDataArray[1].Take(6).ToArray();
                    // Take date of birth check digit from middle line
                    var dateOfBirthCheckChar = fullDataArray[1].Skip(6).Take(1).ToArray();

                    int dateOfBirthCheckDigit;
                    bool correctDateOfBirthCheckDigit = false;
                    if (int.TryParse(dateOfBirthCheckChar, out dateOfBirthCheckDigit))
                    {
                        correctDateOfBirthCheckDigit = MRZScanUtils.CheckDigit(dateOfBirth, dateOfBirthCheckDigit);
                    }
                    else
                    {
                        Console.WriteLine("Parsing of check digit for date of birth failed!");
                    }
                    #endregion

                    #region DateOfExpiry

                    // Take date of expiry from middle line
                    var dateOfExpiry = fullDataArray[1].Skip(8).Take(6).ToArray();
                    // Take date of expiry check digit from middle line
                    var dateOfExpiryCheckChar = fullDataArray[1].Skip(14).Take(1).ToArray();

                    int dateOfExpiryCheckDigit;
                    bool correctDateOfExpiryCheckDigit = false;
                    if (int.TryParse(dateOfExpiryCheckChar, out dateOfExpiryCheckDigit))
                    {
                        correctDateOfExpiryCheckDigit = MRZScanUtils.CheckDigit(dateOfExpiry, dateOfExpiryCheckDigit);
                    }
                    else
                    {
                        Console.WriteLine("Parsing of check digit for date of birth failed!");
                    }
                    #endregion

                    #region OverallCheckDigit
                    // Take full upper line
                    var fullUpperLineMRZ = fullDataArray[0];
                    // Take full middle line
                    var fullMiddleLineMRZ = fullDataArray[1];

                    // Remove overall check digit from middle line
                    var overallCheckChar = fullMiddleLineMRZ.Substring(fullMiddleLineMRZ.Length - 1);

                    int overallCheckDigit;
                    bool correctOverallCheckDigit = false;
                    if (int.TryParse(overallCheckChar, out overallCheckDigit))
                    {
                        // Skip 5 first characters, save the rest
                        var upperLineMRZ = fullUpperLineMRZ.Skip(5).ToArray();

                        // Middle line consists of date of birth, gender, expiry date, nationality, rest of the data, overall check number digit
                        var restOfMiddleLineData = fullMiddleLineMRZ.Skip(18).ToArray();
                        // Remove overall check digit from rest of the data
                        Array.Resize(ref restOfMiddleLineData, restOfMiddleLineData.Length - 1);

                        // Allocate the size of new array that will hold all information for calculating is the overall check digit correct
                        var middleLineMRZ = new char[dateOfBirth.Length
                            + dateOfBirthCheckChar.Length
                            + dateOfExpiry.Length
                            + dateOfExpiryCheckChar.Length
                            + restOfMiddleLineData.Length
                            ];

                        // Copy all relevant data to one single array
                        dateOfBirth.CopyTo(middleLineMRZ, 0);
                        dateOfBirthCheckChar.CopyTo(middleLineMRZ, dateOfBirth.Length);
                        dateOfExpiry.CopyTo(middleLineMRZ, dateOfBirth.Length + dateOfBirthCheckChar.Length);
                        dateOfExpiryCheckChar.CopyTo(middleLineMRZ, dateOfBirth.Length + dateOfBirthCheckChar.Length + dateOfExpiry.Length);
                        restOfMiddleLineData.CopyTo(middleLineMRZ, dateOfBirth.Length + dateOfBirthCheckChar.Length + dateOfExpiry.Length + dateOfExpiryCheckChar.Length);

                        // Concatenate all the relevant data from upper and middle line
                        var overallCheckDigitData = new char[upperLineMRZ.Length + middleLineMRZ.Length];
                        upperLineMRZ.CopyTo(overallCheckDigitData, 0);
                        middleLineMRZ.CopyTo(overallCheckDigitData, upperLineMRZ.Length);

                        correctOverallCheckDigit = MRZScanUtils.CheckDigit(overallCheckDigitData, overallCheckDigit);

                    }
                    else
                    {
                        Console.WriteLine("Failed to parse overall check character to digit");
                    }

                    #endregion

                    #region ISVALID

                    // Assuming isValid means that all the check digit calculations returned the same number as provided
                    if (correctDocumentNumberCheckDigit && correctDateOfBirthCheckDigit
                        && correctDateOfExpiryCheckDigit && correctOverallCheckDigit)
                    {
                        isValid = true;
                    }

                    #endregion

                    #endregion

                    #region DATA EXTRACTION

                    #region FullName

                    var fullLowerLineMRZ = fullDataArray[2];
                    var (firstName, lastName) = MRZScanUtils.ExtractFirstAndLastName(fullLowerLineMRZ);

                    #endregion

                    #region Date
                    var stringDateOfBirth = new string(dateOfBirth);
                    var userDateOfBirth = MRZScanUtils.ExtractDate(stringDateOfBirth);

                    #endregion


                    #endregion

                    #region SAVEUSER

                    var newUser = new User();
                    newUser.FirstName = firstName;
                    newUser.LastName = lastName;
                    newUser.DateOfBirth = userDateOfBirth;
                    newUser.MZRScan = true;
                    newUser.IsValid = isValid;

                    await _apiDbContext.Users.AddAsync(newUser);

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                        return $"First name: {firstName}\nLast name: {lastName}\nDate of birth: {userDateOfBirth}\nIsValid: {isValid}";
                    else
                        return $"User was not successfully saved to the database!";

                    #endregion
                }
                return $"Something went wrong with sending the request or recieving data from it, please try again.";
            }
            return $"Something went wrong with parsing the [{imageFilepath}], please try again with same or different image";
        }

        #region Private Methods
        private async Task<string> CreateRequestAsync(string url, string imageBase64)
        {
            var requestBody = new { type = "MRTD", imageBase64 = imageBase64 };
            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerValue);
            var response = await Client.PostAsync(url, new StringContent(jsonRequestBody, Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            else
            {
                return "";
            }
        }

        private string ParseImageToBase64(string pictureFilePath)
        {
            try
            {
                var ret = Convert.ToBase64String(System.IO.File.ReadAllBytes(imageFilepath));
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.StackTrace}");
                return "";
            }

        }
        #endregion
    }
}
