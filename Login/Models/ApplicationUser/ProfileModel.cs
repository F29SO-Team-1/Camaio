using Login.Models.Threadl;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Login.Models.ApplicationUser
{
    public class ProfileModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string UserRating { get; set; }
        [Required]
        public string ProfileImageUrl { get; set; }
        public DateTime MemmberSince { get; set; }
        public IFormFile ImageUpload { get; set; }

        public IEnumerable<ThreadModel> Threads {get;set;}
        
        public IEnumerable<ChannelModel> Channels { get; set; }

        public IEnumerable<Thread> Likes { get; set; }
    }
}
