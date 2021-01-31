using System;
using System.Collections.Generic;
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
        public string Creator { get; set; }
        public string Description { get; set; }
        public bool Public { get; set; }
        public bool VisibleToGuests { get; set; }
        public bool MembersCanPost { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ChannelMember> ChannelMembers { get; set; }

    }
}