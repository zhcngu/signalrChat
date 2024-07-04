using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

        public async Task Join(string username, string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
           // await Clients.All.SendAsync("ReceiveMessage", username, $"you friend {username} join chat room");

            await Clients.AllExcept(Context.ConnectionId ?? "").SendAsync("ReceiveMessage", username, $"you friend {username} join chat room--base!");

            await Clients.Caller.SendAsync("ReceiveMessage", username, $"Welcome {username} join chat  your id is {Context.ConnectionId}--base!");
            ChatClients.AddClient(Context.ConnectionId, username, group);
        }
        public async Task SendMessage(string user, string message)
        {
            //var exceptClient=  ChatClients.GetClient(item => item.Name == user);
            //await Clients.AllExcept(exceptClient.Id??"").SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message+ "--base!");
        }
        public override Task OnConnectedAsync()
       {
            string connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;
            ChatClients.RemoveClient(connectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
