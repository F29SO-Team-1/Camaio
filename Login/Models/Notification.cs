using System;
using Login.Areas.Identity.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Login.Models.ApplicationUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Login.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public LoginUser User { get; set; }
        public string ThreadId { get; set; }
        public Thread Thread { get; set; }
        public string ChannelId { get; set; }
        public Channel Channel { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
        public DateTime Date { get; set; }

    }
}