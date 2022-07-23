using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRoles")]
        [HttpGet("users-with-roles")]

        public async Task<ActionResult> GetUserswithRoles()
        {
            var users = await _userManager.Users
            .Include( u => u.UserRole)
            .ThenInclude( u => u.Role)
            .OrderBy( u => u.UserName)
            .Select( u => new {
                u.Id,
                Username = u.UserName,
                Roles = u.UserRole.Select( u => u.Role.Name).ToList()
            }).ToListAsync();

            return Ok(users);
        }
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditUserRoles(string username, [FromQuery] string  roles)
        {
            var selectedroles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if(user == null) return NotFound("unable to find user");

            var userroles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,selectedroles.Except(userroles));

            if(!result.Succeeded) return BadRequest("failed to add roles to user");

            result = await _userManager.RemoveFromRolesAsync(user,userroles.Except(selectedroles));

            if(!result.Succeeded) return BadRequest(" Failed to remove from user roles");

            return Ok(await _userManager.GetRolesAsync(user));




        } 

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photos-to-moderate")]

        public ActionResult GetPhotosforModeration()
        {
            return Ok("only admins or moderators can see this");
        }

    }
}