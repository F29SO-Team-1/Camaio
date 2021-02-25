using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Login.Models;

namespace Login.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the LoginUser class
    public class LoginUser : IdentityUser
    {
        public string ProfileImageUrl { get;  set; }
        public int Ratting { get;  set; }
        public DateTime MemberSince { get; internal set; }
        public ICollection<ChannelMember> ChannelMembers { get; set; }
        public IEnumerable<Following> FollowsUser { get; set; }
        public IEnumerable<AchievementModel> Achievements { get; set; }
        public int AccountWarnings { get; set; }
    }
}
