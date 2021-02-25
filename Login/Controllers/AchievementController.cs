using Login.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Controllers
{
    public class AchievementController : Controller
    {
        private readonly IAchievement _achievement;
        public AchievementController(IAchievement achievement)
        {
            _achievement = achievement;
        }
        [Route("{username}/Achievements")]
        public IActionResult Index(string username)
        {
            //build model
            var model = new ProfileModel()
            {

            };
            return View(model);
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

    }
}
