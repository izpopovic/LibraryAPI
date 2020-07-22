using LibraryAPI.Models;
using LibraryAPI.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private ApiDbContext _apiDbContext;
        public UsersController(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public IActionResult Get()
        {
            var users = _apiDbContext.Users;
            if (users == null)
                return NotFound("Users not found!");

            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _apiDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User [{id}] not found!");

            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public IActionResult Post([FromBody] UserView userView)
        {
            var user = new User();

            user.FirstName = userView.FirstName;
            user.LastName= userView.LastName;
            user.DateOfBirth= userView.DateOfBirth;

            _apiDbContext.Users.Add(user);

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status201Created);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserView userView)
        {
            var user = _apiDbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User [{id}] not found!");

            user.FirstName = userView.FirstName;
            user.LastName = userView.LastName;
            user.DateOfBirth = userView.DateOfBirth;

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _apiDbContext.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound($"User [{id}] not found!");

            var userContacts = user.Contacts;
            if (userContacts != null)
                _apiDbContext.Contacts.RemoveRange(userContacts);
            
            _apiDbContext.Users.Remove(user);

            if (_apiDbContext.SaveChanges() > 0)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
