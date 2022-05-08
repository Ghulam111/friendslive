using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedData(DataContext context){

            if(await context.Users.AnyAsync()) return;
            
            var userData  = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                user.passwordSalt = hmac.Key;

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
       }
    }
}