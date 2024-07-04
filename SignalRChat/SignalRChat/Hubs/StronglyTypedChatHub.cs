using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        public async Task Join(string username,   string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.AllExcept(Context.ConnectionId ?? "").Notification( username, $"you friend {username} join chat room--StronglyTypedChatHub");
            await Clients.Caller.ReceiveMessage( username, $"Welcome {username} join chat  your id is {Context.ConnectionId}  StronglyTypedChatHub ");
            ChatClients.AddClient(Context.ConnectionId, username, group);
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message+ "  StronglyTypedChatHub");
        }

        public async Task SendMessageToCaller(string user, string message)
        { await Clients.Caller.ReceiveMessage(user, message+ "  StronglyTypedChatHub"); }

        public async Task SendMessageToGroup(string user, string message)
        {
            await Clients.Group("winform").ReceiveMessage(user, message + "  StronglyTypedChatHub");
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
