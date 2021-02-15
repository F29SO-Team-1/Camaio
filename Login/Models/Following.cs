﻿using Login.Areas.Identity.Data;
using System.Collections.Generic;

namespace Login.Models
{
    public class Following
    {
        public int Id { get; set; }
        public string Username { get; set; } //user follows
        public string FollowingUsers { get; set; } //which user they are following
    }
}
