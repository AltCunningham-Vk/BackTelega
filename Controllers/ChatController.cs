using BackTelega.Models;
using BackTelega.Models.DTOs;
using BackTelega.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackTelega.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly RedisCacheService _cacheService;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(10);

        public ChatController(IChatService chatService, RedisCacheService cacheService)
        {
            _chatService = chatService;
            _cacheService = cacheService;
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(int id)
        {
            var chat = await _chatService.GetChatByIdAsync(id);
            if (chat == null) return NotFound();
            return Ok(new ChatResponse
            {
                Id = chat.Id,
                ChatName = chat.ChatName,
                ChatType = chat.ChatType
                //CreatedAt = chat.CreatedAt
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var cacheKey = "all_chats";
            

            var cachedChats = await _cacheService.GetCacheAsync<IEnumerable<Chat>>(cacheKey);

            if (cachedChats != null)
                return Ok(cachedChats);

            var chats = await _chatService.GetAllChatsAsync();
            return Ok(chats.Select(chat => new ChatResponse
            {
                Id = chat.Id,
                ChatName = chat.ChatName,
                ChatType = chat.ChatType
                //CreatedAt = chat.CreatedAt
            }));
            await _cacheService.SetCacheAsync(cacheKey, chats, _cacheExpiry);
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatRequest request)
        {
            bool result = await _chatService.CreateChatAsync(request.ChatName, request.ChatType);
            if (!result) return BadRequest("Invalid chat type");
            return Ok("Chat created successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            bool result = await _chatService.DeleteChatAsync(id);
            if (!result) return NotFound();
            return Ok("Chat deleted successfully");
        }
    }
}
