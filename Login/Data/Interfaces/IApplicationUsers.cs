﻿using Login.Areas.Identity.Data;
using Login.Models;
using Login.Models.Threadl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IApplicationUsers
    {
        LoginUser GetById(string id);

        LoginUser GetByUserName(string username);
        IEnumerable<LoginUser> GetAll();

        int GetRatting(string username, IEnumerable<ThreadModel> threadList);
        void UpdateUser(LoginUser user);

        Task SetProfileImage(string id, Uri uri);

        Task Follows(string userA, string userB);

        IEnumerable<Following> UsersFollower(string username);
    }
}
