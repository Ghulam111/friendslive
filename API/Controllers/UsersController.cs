
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _dbContext;
        public UsersController(DataContext datacontext)
        {
            _dbContext = datacontext;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){

           return await _dbContext.Users.ToListAsync();
            
            
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id){

            return await _dbContext.Users.FindAsync(id);
            
            
        }

    }
}