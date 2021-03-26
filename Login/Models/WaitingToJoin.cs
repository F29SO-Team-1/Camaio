using System;
using System.Collections.Generic;
using Login.Areas.Identity.Data;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Login.Models.ApplicationUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Login.Models
{
    public class WaitingToJoin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public LoginUser User { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

    }
}