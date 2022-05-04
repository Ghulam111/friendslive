
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;

        public AccountController(DataContext context, ITokenService tokenservice)
        {
            _tokenservice = tokenservice;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<userDto>> Register(RegisterDTO registeruser)
        {
            if (await UserExists(registeruser.username)) return BadRequest("username already exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registeruser.username.ToLower(),
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registeruser.password)),
                passwordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new userDto
            {
                Username = user.UserName,
                Token = _tokenservice.createToken(user)
            };

        }
        [HttpPost("login")]
        public async Task<ActionResult<userDto>> Login(loginDto logindto)
        {
            var loginuser = await _context.Users.SingleOrDefaultAsync(u => u.UserName == logindto.Username);

            if (loginuser == null)
            {
                return Unauthorized("invalid username");
            }
            using var hmac = new HMACSHA512(loginuser.passwordSalt);

            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (loginuser.passwordHash[i] != ComputedHash[i]) return Unauthorized("Invalid password");
            }

            return new userDto 
            {
                Username = loginuser.UserName,
                Token  = _tokenservice.createToken(loginuser)
            };



        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());

        }
    }
}