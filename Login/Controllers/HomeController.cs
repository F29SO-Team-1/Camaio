using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Login.Data;
using Login.Models.Threadl;
using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Login.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IThread _threadService;
        private readonly IAchievement _achievementService;
        private readonly UserManager<LoginUser> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            IThread thread, 
            IAchievement achievementService,
            UserManager<LoginUser> userManager)
        {
            _logger = logger;
            _threadService = thread;
            _achievementService = achievementService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var threadModel = _threadService.GetAll().Select(threads => new ThreadModel
            {
                Title = threads.Title,
                Rating = threads.Votes,
                Description = threads.Description,
                Created = threads.CreateDate,
                Picture = threads.Image,
                Id = threads.ID,
                AuthorUserName = threads.UserName
            });

            var threadList= new ThreadList { ThreadLists = threadModel };
            return View(threadList);
        }

        
    }
}
