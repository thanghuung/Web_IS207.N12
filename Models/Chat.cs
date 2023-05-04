using System.Collections.Generic;

namespace WEB2.Models
{
    public class Chat
    {
        //public Chat()
        //{
        //    Messages = new List<Message>();
        //    Users = new List<ChatUser>();
        //}

        public string Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Users { get; set; }
    }
}