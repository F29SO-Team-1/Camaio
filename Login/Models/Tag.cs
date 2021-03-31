namespace Login.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public int ThreadId { get; set; }
        public Thread Thread { get; set; }

    }
}