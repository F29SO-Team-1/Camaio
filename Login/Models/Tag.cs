namespace Login.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChannelId { get; set; }
        public Channel Channel { get; set; }
        public string ThreadId { get; set; }
        public Thread Thread { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }

    }
}