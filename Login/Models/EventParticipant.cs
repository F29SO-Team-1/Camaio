using Login.Areas.Identity.Data;


namespace Login.Models
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public LoginUser User { get; set; }

    }
}