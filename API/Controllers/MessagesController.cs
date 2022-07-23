using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
       
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            

        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(MessageDto CreatemessageDto)
        {
            var logedinUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(logedinUser);

            if (Sender.UserName == CreatemessageDto.recipientUsername) return BadRequest("you cannot message yourself");

            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(CreatemessageDto.recipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = Sender,
                Recipient = recipient,
                SenderUsername = Sender.UserName,
                RecipietUsername = recipient.UserName,
                Content = CreatemessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Creation of message failed");
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesforUser([FromQuery] MessageParams messageParams)
        {
            var logedinUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(logedinUser);

            messageParams.Username = logedinUser;

            var messages = await _unitOfWork.MessageRepository.GetMessagesForUserAsync(messageParams);

            Response.addPaginationHeaders(messages.CurrentPage, messages.TotalPages, messages.TotalCount, messages.PageSize);

            return messages;
        }

        [HttpGet("thread/{username}")]

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesThread(string username)
        {
            var logedinUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var messages = await _unitOfWork.MessageRepository.GetMessagesThread(logedinUser, username);

            return Ok(messages);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var Username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var message = await _unitOfWork.MessageRepository.GetMessage(id);

            if (message.Sender.UserName != Username && message.Recipient.UserName != Username) return Unauthorized();

            if (message.Sender.UserName == Username) message.SenderDeleted = true;

            if (message.Recipient.UserName == Username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
            {
                _unitOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete message");
        }
    }
}