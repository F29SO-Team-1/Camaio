using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
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
        private readonly IAchievement _service;
        public AchievementController(IAchievement service)
        {
            _service = service;
        }
        [Route("{username}/Achievements")]
        public IActionResult Index(string username)
        {
            var usersAchiev = _service.GetAllAchievements();
            //var listOfAchiev = BuildAchievementsList(username);

            //build model
            var model = _service.GetAllAchievements().Select(achiev => new AchievementModel
            {
                Name = achiev.Name,
                Description = achiev.Description,
                Picture = achiev.Picture
            });

            var usersAchievementList = new AchievementModelList { AchievementLists = model };
            return View(usersAchievementList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        //makes the model to me passed in the view
        /*private IEnumerable<AchievementModel> BuildAchievementsList(LoginUser userName)
        {
            return _service.UsersAchievements(userName).Select(achievement => new AchievementModel
            {
                Name = achievement.Name,
                Description = achievement.Description,
                Picture = achievement.Picture
            });
        }*/
    }
}
