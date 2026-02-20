namespace signalRMVC.Models
{
    public class User
    {
        public string Name { get; set; }
        public List<string> Inbox { get; set; } = new List<string>();

        public User(string name)
        {
            Name = name;
        }

        public void ReceiveMessage(string sender, string message)
        {
            Inbox.Add($"{sender}: {message}");
        }
    }
}
