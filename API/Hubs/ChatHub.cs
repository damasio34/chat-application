using ChatApplication.Core.Domain;
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
        public Task AddToRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task SendMessageToRoom(Message message)
        {
            message.Id = Guid.NewGuid().ToString();
            message.When = DateTime.UtcNow;

            return Clients.Groups(message.Room).SendAsync("receiveMessage", message);
        }
    }
}
