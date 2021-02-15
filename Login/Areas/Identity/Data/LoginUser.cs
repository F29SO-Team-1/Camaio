﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Thread Likes { get; set; }
    }
}
