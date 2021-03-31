using Login.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Login.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the LoginUser class
    public class LoginUser : IdentityUser
    {
        public string ProfileImageUrl { get; set; }
        public int Ratting { get; set; }
        public DateTime MemberSince { get; internal set; }
        public ICollection<ChannelMember> Channels { get; set; }
        public IEnumerable<EventParticipant> Events { get; set; }
        public IEnumerable<Following> FollowsUser { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }
        public IEnumerable<Channel> CreatedChannels { get; set; }
        public IEnumerable<Event> CreatedEvents { get; set; }
        public IEnumerable<WaitingToJoin> PendingRequests { get; set; }
        public int AccountWarnings { get; set; }
    }
}
