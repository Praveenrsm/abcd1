using Microsoft.AspNetCore.SignalR;
using signalRMVC.Models;

namespace signalRMVC
{
    public sealed class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}



        // In-memory list of users
        private static readonly List<User> users = new List<User>
        {
            new User("Alice"),
            new User("Bob"),
            new User("Charlie"),
            new User("Dave")
        };

        // Map username to connection ID
        private static readonly Dictionary<string, string> userConnections = new Dictionary<string, string>();

        public void RegisterUser(string username)
        {
            if (!userConnections.ContainsKey(username))
                userConnections.Add(username, Context.ConnectionId);
        }

        // Send private message to 1 user
        public async Task SendPrivateMessage(string sender, string receiver, string message)
        {
            var receiverUser = users.FirstOrDefault(u => u.Name == receiver);
            var senderUser = users.FirstOrDefault(u => u.Name == sender);

            if (receiverUser != null)
            {
                receiverUser.ReceiveMessage(sender, message);
            }

            if (senderUser != null)
            {
                senderUser.ReceiveMessage(sender, message); // Add to sender inbox too
            }

            // Send to both sender and receiver connections
            if (userConnections.ContainsKey(receiver))
            {
                string receiverConn = userConnections[receiver];
                await Clients.Client(receiverConn).SendAsync("ReceiveMessage", sender, message);
            }

            if (userConnections.ContainsKey(sender))
            {
                string senderConn = userConnections[sender];
                await Clients.Client(senderConn).SendAsync("ReceiveMessage", sender, message);
            }
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (user != null)
                userConnections.Remove(user);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
