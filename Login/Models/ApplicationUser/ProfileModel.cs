using Login.Models.Threadl;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Login.Models.ApplicationUser
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int UserRating { get; set; }
        [Required]
        public string ProfileImageUrl { get; set; }
        public DateTime MemmberSince { get; set; }
        public IFormFile ImageUpload { get; set; }
        public int Warnings { get; set; }

        public Task<IList<string>> Roles { get; set; }
        public IEnumerable<ThreadModel> Threads { get; set; }

        public IEnumerable<ChannelModel> Channels { get; set; }

        public IEnumerable<Thread> Likes { get; set; }

        public IEnumerable<Following> FollowsUser { get; set; } //which users the person follows

        public IEnumerable<Following> UsersFollowed { get; set; } //which people follow this user
    }
}
