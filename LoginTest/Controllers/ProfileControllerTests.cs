using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.ApplicationUser;
using Login.Models.Followers;
using Login.Models.Threadl;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace Login.Controllers.Tests
{
    [TestClass]
    public class ProfileControllerTests
    {
        [TestMethod]
        public void ProfileControllerTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void IndexTest()
        {
            var controller = new ProfileController();
        }

        [TestMethod()]
        public void FollowTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ScoresTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UploadProfileImageTest()
        {
            Assert.Fail();
        }
    }
}