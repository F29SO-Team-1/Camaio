using System.Collections.Generic;


namespace Login.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public bool VisibleToGuests { get; set; }
        public bool MembersCanPost { get; set; }
        public IEnumerable<Thread> Threads { get; set; }

    }
}