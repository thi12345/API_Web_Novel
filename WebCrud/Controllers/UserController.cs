using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCrud.Data;
using WebCrud.Entities;

namespace WebCrud.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController(DataContext context) : BaseApiController
{
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();
            return users;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var users = await context.Users.FindAsync(id);
            if (users is null) return NotFound();
            return users;
        }
    }

