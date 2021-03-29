using Login.Models.Threadl;
using Login.Models.ApplicationUser;
using System.Collections.Generic;


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
