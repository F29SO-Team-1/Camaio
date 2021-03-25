using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models.Threadl;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Login.Controllers
{

    public class HomeController : Controller
    {
        private readonly IThread _threadService;
        private readonly IAchievement _achievementService;
        private readonly UserManager<LoginUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            IThread thread,
            IAchievement achievementService,
            UserManager<LoginUser> userManager)
        {
            _threadService = thread;
            _achievementService = achievementService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var threadModel = _threadService.GetAll()
                .Where(threads => threads.AlbumId == 1)
                .Select(threads => new ThreadModel
                {
                    Title = threads.Title,
                    Rating = threads.Votes,
                    Description = threads.Description,
                    Created = threads.CreateDate,
                    Picture = threads.Image,
                    Id = threads.ID,
                    AuthorUserName = threads.UserName,
                    Flagged = threads.Flagged
                });

            var threadList = new ThreadList { ThreadLists = threadModel };
            return View(threadList);
        }


    }
}
