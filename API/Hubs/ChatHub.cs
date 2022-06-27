using ChatApplication.Core.Domain;
using ChatApplication.Core.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ChatApplication.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IBrockerService _brockerService;

        public ChatHub(IBrockerService brockerService)
        {
            this._brockerService = brockerService;
        }

        public Task AddToRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task RemoveToRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public Task SendMessageToRoom(ChatMessage chatMessage)
        {
            chatMessage.Id = Guid.NewGuid().ToString();
            chatMessage.When = DateTime.UtcNow;
            chatMessage.ConnectionId = Context.ConnectionId;

            const string stockCode = "/stock=";

            if (chatMessage.Text.StartsWith(stockCode))
            {
                chatMessage.MessageCode = new MessageCode 
                {
                    Code = stockCode,
                    Value = chatMessage.Text.Substring(7, chatMessage.Text.Length - 7)
                };
                this._brockerService.SendMessage(chatMessage, "api-to-bot");

                return Groups.AddToGroupAsync(Context.ConnectionId, chatMessage.ConnectionId);
            }

            return Clients.Groups(chatMessage.Room)?.SendAsync("receiveMessage", chatMessage);
        }
    }
}
