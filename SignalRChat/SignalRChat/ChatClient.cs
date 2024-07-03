using System.Collections.Concurrent;

namespace SignalRChat
{
    public   class SignalRChatClient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public int Stars { get; set; }
    }

    public static  class ChatClients
    {
        private static   List< SignalRChatClient>  clients = new List<SignalRChatClient> ();
        public  static void AddClient(string id,string userName,string  group)
        {
            var client=new SignalRChatClient() {  Id=id, Name=userName, Group=group };
              clients.Add(client);
        }
        public static void RemoveClient(string clientId)
        {

            clients.Remove(GetClient(clientId));
          
        }
        public static SignalRChatClient GetClient(string clientId)
        {
            return clients.FirstOrDefault(item => item.Id == clientId);
        }
        public static SignalRChatClient GetClient(Func<SignalRChatClient, bool> query)
        {
            var client = clients.FirstOrDefault(query);
            return client;
        }
        public static List<SignalRChatClient> GetAll()
        {
            return clients; 
        }
    }
}
