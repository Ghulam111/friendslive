using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedData(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager){

            if(await userManager.Users.AnyAsync()) return;
            
            var userData  = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;
            
            var roles = new List<AppRole>
            {
                new AppRole{Name="Member"},
                new AppRole{Name="Admin"},
                new AppRole{Name="Moderator"}
            };
            foreach ( var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users)
            {
                // using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                // user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                // user.passwordSalt = hmac.Key;

               await userManager.CreateAsync(user,"P@ssw0rd");
               await userManager.AddToRoleAsync(user,"Member");
            }
            var admin = new AppUser
            {
                UserName = "admin"
            };
            await userManager.CreateAsync(admin,"P@ssw0rd");
            await userManager.AddToRolesAsync(admin, new[] {"Admin","Moderator"});

            
       }
    }
}