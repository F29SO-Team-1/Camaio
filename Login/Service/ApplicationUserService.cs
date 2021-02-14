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

        public int GetRatting(string username, IEnumerable<ThreadModel> threadList)
        {
            var user = GetByUserName(username);
            foreach (var post in threadList)
            {
                user.Ratting += post.Rating;
            }
            return user.Ratting;
        }

        public async Task SetProfileImage(string username, Uri uri)
        {
            var user = GetByUserName(username);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
