using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Login.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the LoginUser class
    public class LoginUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        /*[PersonalData]
        public Object Avatar { get; set; }*/
    }
}
