namespace WEB2.Models
{
    public class ChatUser
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string ChatId { get; set; }
        public Chat Chat { get; set; }
        public UserRole Role { get; set; }
    }
}