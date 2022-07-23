
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;

        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManger, ITokenService tokenservice, IMapper mapper)
        {
            _signInManger = signInManger;
            _userManager = userManager;
            _mapper = mapper;
            _tokenservice = tokenservice;
            

        }

        [HttpPost("register")]
        public async Task<ActionResult<userDto>> Register(RegisterDTO registeruser)
        {
            if (await UserExists(registeruser.username)) return BadRequest("username already exists");

            var user = _mapper.Map<AppUser>(registeruser);

            using var hmac = new HMACSHA512();


            user.UserName = registeruser.username.ToLower();
            // user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registeruser.password));
            // user.passwordSalt = hmac.Key;


           var result = await _userManager.CreateAsync(user,registeruser.password);
           if (!result.Succeeded) return BadRequest(result.Errors);
            var roleResult = await _userManager.AddToRoleAsync(user,"Member");
           if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new userDto
            {
                Username = user.UserName,
                Token = await  _tokenservice.createToken(user),
                knownAs = user.knownAs,
                Gender = user.gender
            };

        }
        [HttpPost("login")]
        public async Task<ActionResult<userDto>> Login(loginDto logindto)
        {
            var loginuser = await _userManager.Users
            .Include(p => p.photos)
            .SingleOrDefaultAsync(u => u.UserName == logindto.Username);

            if (loginuser == null)
            {
                return Unauthorized("invalid username");
            }

            var result = await _signInManger.CheckPasswordSignInAsync(loginuser,logindto.Password,false);
          
            if(!result.Succeeded) return Unauthorized();

            return new userDto
            {
                Username = loginuser.UserName,
                Token = await _tokenservice.createToken(loginuser),
                PhotoUrl = loginuser.photos.FirstOrDefault(x => x.IsMain)?.Url,
                knownAs = loginuser.knownAs,
                Gender = loginuser.gender

            };



        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());

        }
    }
}