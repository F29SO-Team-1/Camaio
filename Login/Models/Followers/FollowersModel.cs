using Login.Areas.Identity.Data;
using System.Collections.Generic;

namespace Login.Models.Followers
{
    public class FollowersModel
    {
        public LoginUser Username { get; set; } //user follows
    }

    public class FollowersModelList
    {
        public IEnumerable<FollowersModel> UserList { get; set; } //user follows
    }
}

