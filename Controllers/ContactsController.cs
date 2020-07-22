using LibraryAPI.Models;
using LibraryAPI.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryAPI.Controllers
{
    [Route("api/Users/{userId}/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private ApiDbContext _apiDbContext;
        public ContactsController(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        // GET: api/<UserContactController>
        [HttpGet]
        public IActionResult Get(int userId)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound($"User[{userId}] not found!");

            return Ok(user.Contacts);
        }

        // GET api/<UserContactController>/5
        [HttpGet("{contactId}")]
        public IActionResult Get(int userId, int contactId)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound($"User[{userId}] not found!");

            var contact = user.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
                return NotFound($"Contact[{contactId}] not found!");

            return Ok(contact);
        }

        // POST api/<UserContactController>
        [HttpPost]
        public IActionResult Post(int userId, [FromBody] ContactView contactView)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound($"User[{userId}] not found!");

            var contact = new Contact();
            contact.FirstName = contactView.FirstName;
            contact.LastName = contactView.LastName;
            contact.PhoneNumber = contactView.PhoneNumber;

            user.Contacts.Add(contact);

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status201Created);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<UserContactController>/5
        [HttpPut("{contactId}")]
        public IActionResult Put(int userId, int contactId, [FromBody] ContactView contactView)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound($"User[{userId}] not found!");

            var contact = user.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
                return NotFound($"Contact[{contactId}] not found!");

            contact.FirstName = contactView.FirstName;
            contact.LastName = contactView.LastName;
            contact.PhoneNumber = contactView.PhoneNumber;

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // DELETE api/<UserContactController>/5
        [HttpDelete("{contactId}")]
        public IActionResult Delete(int userId, int contactId)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound($"User[{userId}] not found!");

            var contact = user.Contacts.FirstOrDefault(c => c.Id == contactId);
            if (contact == null)
                return NotFound($"Contact[{contactId}] not found!");

            user.Contacts.Remove(contact);

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
