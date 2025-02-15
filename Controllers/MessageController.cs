using BackTelega.Hubs;
using BackTelega.Models;
using BackTelega.Models.DTOs;
using BackTelega.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BackTelega.Controllers
{
    [Authorize]
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public MessageController(IMessageService messageService, IHubContext<ChatHub> chatHub)
        {
            _messageService = messageService;
            _chatHub = chatHub;
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(int chatId)
        {
            var messages = await _messageService.GetMessagesByChatIdAsync(chatId);

            return Ok(messages.Select(msg => new MessageResponse
            {
                Id = msg.Id, // Преобразуем nullable int в int
                ChatId = msg.ChatId ?? 0, // Преобразуем nullable int в int
                SenderId = msg.SenderId ?? 0, // Преобразуем nullable int в int
                MessageText = msg.MessageText,
                MediaType = msg.MediaType,
                MediaUrl = msg.MediaUrl,
                //SentAt = msg.SentAt ?? DateTime.UtcNow // Преобразуем nullable DateTime в DateTime
            }));
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            bool result = await _messageService.SendMessageAsync(request.ChatId, request.SenderId, request.MessageText, request.MediaType, request.MediaUrl);
            if (!result) return BadRequest("Invalid message format");

            // Отправляем сообщение в SignalR
            await _chatHub.Clients.Group(request.ChatId.ToString()).SendAsync(
                "ReceiveMessage",
                request.ChatId,
                request.SenderId,
                request.MessageText,
                request.MediaType,
                request.MediaUrl
            );

            return Ok("Message sent successfully");
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")] // Добавляем поддержку загрузки файлов
        public async Task<IActionResult> UploadMessageFile([FromForm] UploadFileRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.File.FileName)}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            // Отправляем файл в SignalR
            await _chatHub.Clients.Group(request.ChatId.ToString()).SendAsync(
                "ReceiveMessage",
                request.ChatId,
                request.SenderId,
                "",
                "file",
                fileUrl
            );

            return Ok(new { fileUrl });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] SendMessageRequest request)
        {
            bool result = await _messageService.UpdateMessageAsync(id, request.MessageText, request.MediaType, request.MediaUrl);
            if (!result) return NotFound("Message not found");
            return Ok("Message updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            bool result = await _messageService.DeleteMessageAsync(id);
            if (!result) return NotFound();
            return Ok("Message deleted successfully");
        }
    }
}
