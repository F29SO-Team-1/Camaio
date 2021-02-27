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
        private readonly IApplicationUsers _userService;
        public AchievementController(IAchievement service, IApplicationUsers userService)
        {
            _service = service;
            _userService = userService;
        }

        [Route("{username}/Achievements")]
        public IActionResult Index(string username)
        {
            LoginUser user = _userService.GetByUserName(username);
            int? usersAch = _service.GetUsersAchievement(user).Count();
            int totalAmountOfAch = _service.GetAllAchievements().Count();
            //checks the number of Achievements compared to the number of users Achievements
            if (usersAch != totalAmountOfAch) _service.AssignAchievementsToUser(user);
            if (usersAch == null) return NotFound();

            //build model
            var model = _service.GetUsersAchievement(user).Select(achiev => new AchievementModel
            {
                Picture = achiev.Achievement.Picture,
                Name = achiev.Achievement.Name,
                Description = achiev.Achievement.Description,
                Progress = achiev.UsersProgress,
                MaxProgress = achiev.MaxProgress,
                CompletedTime = achiev.CompletedTime,
                Completed = achiev.Completed
            });

            var usersAchievementList = new AchievementModelList { AchievementLists = model };
            return View(usersAchievementList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

    }
}
