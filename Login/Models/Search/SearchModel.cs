using Login.Areas.Identity.Data;
using Login.Models.Threadl;
using Login.Models.ApplicationUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Login.Models.Search
{
    public class SearchModel
    {
        public bool ThreadsIncluded { get; set; }
        public bool ChannelsIncluded { get; set; }
        public bool UsersIncluded { get; set; }
        public IEnumerable<ThreadModel> Threads { get; set; }
        public IEnumerable<ChannelModel> Channels { get; set; }
        public IEnumerable<ProfileModel> Users { get; set; }
    }
}
