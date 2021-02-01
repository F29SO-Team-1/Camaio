using Login.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IApplicationUsers
    {
        LoginUser GetById(string id);

        LoginUser GetByUserName(string username);
        IEnumerable<LoginUser> GetAll();

        Task IncrementRating(string id, Type type);

        Task SetProfileImage(string id, Uri uri);
    }
}
