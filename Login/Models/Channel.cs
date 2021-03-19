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
    public class Channel
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public LoginUser Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ChannelMember> ChannelMembers { get; set; }
        public ICollection<Album> Albums { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }

    }
}