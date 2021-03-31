using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.Threadl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class ApplicationUserService : IApplicationUsers
    {
        private readonly LoginContext _context;
        public ApplicationUserService(LoginContext content)
        {
            _context = content;
        }

        public IEnumerable<LoginUser> GetAll()
        {
            return _context.Users;
        }

        public LoginUser GetById(string id)
        {
            return GetAll().FirstOrDefault(u => u.Id == id);
        }

        public LoginUser GetByUserName(string username)
        {
            return GetAll().FirstOrDefault(u => u.UserName == username);
        }

        public bool IfUserExists(string username)
        {
            return _context.Users.Any(x => x.UserName == username);
        }

        public int GetRatting(string username, IEnumerable<ThreadModel> threadList)
        {
            var user = GetByUserName(username);
            user.Ratting = 0;
            foreach (var post in threadList)
            {
                user.Ratting += post.Rating;
            }
            UpdateUser(user);
            return user.Ratting;
        }

        public void UpdateUser(LoginUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public async Task SetProfileImage(string username, Uri uri)
        {
            var user = GetByUserName(username);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Follows(string userA, string userB)
        {
            var fromUser = GetByUserName(userA);
            var wantsToFollow = GetByUserName(userB);

            //new instance of follow
            var follow = new Following
            {
                Username = fromUser.UserName,
                FollowingUsers = wantsToFollow
            };

            //check if the user already follows this user
            foreach (Following f in UsersFollowers(wantsToFollow))
            {
                //the user cannot follow them selfs 
                //and cannot keep following if already on the list
                if (fromUser == wantsToFollow || f.Username == fromUser.UserName) return;
            }

            _context.Add(follow);

            await _context.SaveChangesAsync();
        }

        //returns a list of users that the user follows
        public IEnumerable<Following> UsersFollowers(LoginUser user)
        {
            return _context.Follow
                .Where(f => f.FollowingUsers == user)
                .ToList();
        }

        //change the lsit of the users to the userLogin DAta type
        public List<LoginUser> UserFollowingList(LoginUser user)
        {
            List<LoginUser> list = new List<LoginUser>();
            var uF = UsersFollowers(user);

            foreach (var t in uF)
            {
                list.Add(GetByUserName(t.Username));
            }

            return list;
        }

        public async Task GiveUserWarning(string userId)
        {
            LoginUser u = GetById(userId);
            u.AccountWarnings += 1;
            await _context.SaveChangesAsync();
        }

        //TODO
        public IEnumerable<LoginUser> ListOfFollowing(LoginUser user)
        {
            return _context.Follow.Where(x => x.Username == user.UserName).Select(x=>x.FollowingUsers);
        }
    }
}

