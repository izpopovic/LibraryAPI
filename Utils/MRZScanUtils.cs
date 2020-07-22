using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LibraryAPI.Utils
{
    public static class MRZScanUtils
    {

        private static Dictionary<char, int> lettersToDigits = DictionaryUtils.FillDictionary();
        private static int[] fullMultiplier = ArrayUtils.CreateMultipliedArray(20);
        private static char filler = '<';
        public static bool CheckDigit(char[] data, int parsedCheckDigit)
        {
            int number;
            int multiplyResult;
            int addingResult = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (Char.IsDigit(data[i]))
                {
                    // it's a number
                    number = (int)Char.GetNumericValue(data[i]);
                }
                else
                {
                    if (!lettersToDigits.TryGetValue(data[i], out number))
                    {
                        // it's a character
                        continue;
                    }
                }

                // 113971223
                // 731731731
                multiplyResult = number * fullMultiplier[i];
                addingResult += multiplyResult;
            }

            var checkDigitDocumentNumberResult = addingResult % 10;
            if (parsedCheckDigit == checkDigitDocumentNumberResult)
            {
                Console.WriteLine($"Calculated and provided check digits match! [{checkDigitDocumentNumberResult}] [{parsedCheckDigit}]");
                return true;
            }
            Console.WriteLine($"Calculated and provided check digits do not match! [{checkDigitDocumentNumberResult}] [{parsedCheckDigit}]");
            return false;
        }

        public static (string, string) ExtractFirstAndLastName(string fullLowerLineMRZ)
        {
            // Change the input for testing purposes
            // fullLowerLineMRZ = "POPOVIC<FERRARA<<IVAN<ZVONIMIR<<<<<";

            string firstName = "";
            string lastName = "";

            bool finishedReadingLastName = false;
            bool finishedReadingFirstName = false;
            int counter = 0;

            for (int i = 0; i < fullLowerLineMRZ.Length; i++)
            {
                if (fullLowerLineMRZ[i] == filler)
                {
                    counter++;
                    if (counter == 1 && finishedReadingLastName)
                    {
                        firstName += " ";
                    }
                    if (counter == 1 && !finishedReadingLastName)
                    {
                        lastName += " ";
                    }
                    continue;
                }
                if (counter >= 2)
                {
                    finishedReadingLastName = true;
                    counter = 0;
                }

                if (!finishedReadingLastName)
                {
                    lastName += fullLowerLineMRZ[i];
                }

                if (finishedReadingLastName)
                {
                    firstName += fullLowerLineMRZ[i];
                }
            }

            return (firstName, lastName);
        }

        public static DateTime ExtractDate(string dateOfBirth)
        {
            var result = DateTime.ParseExact(dateOfBirth, "yyMMdd", CultureInfo.InvariantCulture);
            return result;
        }
    }
}
