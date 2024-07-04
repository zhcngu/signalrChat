namespace SignalRChat.Hubs
{
    public interface IChatClient
    {
        Task Notification(string user,string message);
        Task ReceiveMessage(string user, string message);
    }
}
