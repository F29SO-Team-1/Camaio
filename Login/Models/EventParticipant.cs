using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Login.Models.ApplicationUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Login.Areas.Identity.Data;


namespace Login.Models
{
    public class EventParticipant
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public Event Event { get; set; }
        public string UserName { get; set; }
        public LoginUser LoginUser { get; set; }

    }
}