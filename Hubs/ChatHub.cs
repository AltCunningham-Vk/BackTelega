using BackTelega.Models;
using BackTelega.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BackTelega.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserStatusService _userStatusService;

        public ChatHub(UserStatusService userStatusService)
        {
            _userStatusService = userStatusService;
        }
        public async Task SendMessage(int chatId, int senderId, string messageText, string? mediaType, string? mediaUrl)
        {
            // Отправляем сообщение всем в этом чате
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", chatId, senderId, messageText, mediaType, mediaUrl);
        }

        public async Task JoinChat(int chatId, int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await _userStatusService.SetUserOnlineAsync(userId);
        }
        public async Task<bool> CheckUserOnline(int userId)
        {
            return await _userStatusService.IsUserOnlineAsync(userId);
        }

        public async Task LeaveChat(int chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
