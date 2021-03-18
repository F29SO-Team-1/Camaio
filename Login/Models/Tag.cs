namespace Login.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Channel Channel { get; set; }
        public Thread Thread { get; set; }
        public Event Event { get; set; }

    }
}