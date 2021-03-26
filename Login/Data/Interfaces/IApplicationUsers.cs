using Login.Areas.Identity.Data;
using Login.Models;
using Login.Models.Threadl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IApplicationUsers
    {
        //gets the users Id
        LoginUser GetById(string id);

        //gets the users name
        LoginUser GetByUserName(string username);

        //gets all users
        IEnumerable<LoginUser> GetAll();

        //gets the ratting of the user
        int GetRatting(string username, IEnumerable<ThreadModel> threadList);
        void UpdateUser(LoginUser user);

        Task SetProfileImage(string id, Uri uri);

        //Adds a user to a following list
        Task Follows(string userA, string userB);

        //returns a list of the users that that user follows
        IEnumerable<Following> UsersFollowers(LoginUser user);

        //Gives a user a warning
        Task GiveUserWarning(string userId);

        bool IfUserExists(string username);

        List<LoginUser> UserFollowingList(LoginUser user);
    }
}
