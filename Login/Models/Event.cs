using Login.Areas.Identity.Data;
using System;
using System.Collections.Generic;


namespace Login.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public LoginUser Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IEnumerable<EventParticipant> Participants { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }

    }
}