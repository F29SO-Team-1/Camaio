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

namespace Login.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IThread _threadService;

        public HomeController(ILogger<HomeController> logger, IThread thread)
        {
            _logger = logger;
            _threadService = thread;
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


        /*[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
