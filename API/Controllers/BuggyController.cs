using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _dbcontext;
        public BuggyController(DataContext  dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [Authorize]
        
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){

            return  "secret key";
        }
        

        
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var user = _dbcontext.Users.Find(-1);

            if(user == null) return NotFound();

            return  Ok(user);
        }

       
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {

           try
           {
            var thing = _dbcontext.Users.Find(-1);

            var thingtoReturn = thing.ToString();

            return thingtoReturn;
           }
           catch (Exception ex)
           {

               return StatusCode(500,"Computer Says No !");
           }
        }
        
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {

            return  BadRequest("secret key");
        }
       
    }
}