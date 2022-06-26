using ChatApplication.Core.Domain;
using ChatApplication.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatApplication.API.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        public Task AddToRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task SendMessageToRoom(Message message)
        {
            message.Id = Guid.NewGuid().ToString();
            message.When = DateTime.UtcNow;

            if (message.Text.StartsWith("/stock="))
            {
                var stockCode = message.Text.Substring(7, message.Text.Length - 7);
                var brockerSenderService = new BrockerSenderService();
                brockerSenderService.SendMessage(stockCode);

                return Groups.AddToGroupAsync(Context.ConnectionId, stockCode);
            }

            return Clients.Groups(message.Room).SendAsync("receiveMessage", message);
        }
    }
}
