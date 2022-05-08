
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;

        private readonly IUserRepository _IUserrepository;
        public UsersController(IUserRepository IUserrepository, IMapper mapper)
        {
            _mapper = mapper;
            _IUserrepository = IUserrepository;


        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users= await _IUserrepository.GetMembersAsync();

            return Ok(users);

            

        }

        // [HttpGet("{id}")]

        // public async Task<ActionResult<AppUser>> GetUser(int id)
        // {

        //     return await _IUserrepository.GetUserByIdAsync(id);


        // }

        [HttpGet("{username}")]
        public async Task<MemberDTO> getuserbyUsername(string username)
        {
            return await _IUserrepository.getMemberAsync(username);
            
        }

    }
}