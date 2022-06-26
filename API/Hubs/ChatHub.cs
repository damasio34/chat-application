using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatApplication.API.Hubs
{
    public class ChatHub : Hub
    {
        public Task AddToRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public Task SendMessage(string roomName, string message)
        {
            return Clients.Groups(roomName).SendAsync(message);
        }
    }
}
