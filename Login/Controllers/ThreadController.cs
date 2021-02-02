using Login.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Login.Models.Threadl;
using System.Threading.Tasks;
using Login.Models;
using Microsoft.AspNetCore.Identity;
using Login.Areas.Identity.Data;
using System;

namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThread _service;
        private readonly ThreadContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<LoginUser> _userManager;

        public ThreadController(IThread thread, ThreadContext context, IConfiguration configuration, UserManager<LoginUser> userManager)
        {
            _service = thread;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        [Route("Thread/{id}")]
        //To view ONE thread whatever
        public IActionResult Index(int id)
        {
            //get the id of the thread
            var thread = _service.GetById(id);

            //TODO Replies

            //make a view model for the thread
            var model = new ThreadModel
            {
                Id = thread.ID,
                AuthorId = thread.UserID,
                Created = thread.CreateDate,
                Description = thread.Description,
                Picture = thread.Image,
                Title = thread.Title
            };

            return View(model);
        }

        // Returns a list of threads 
        public IActionResult ThreadList()
        {
            var threads = _service.GetAll().Select(t => new ThreadModel
            {
                Id = t.ID,
                Title = t.Title,
                Description = t.Description
            });

            var model = new ThreadList
            {
                ThreadLists = threads
            };

            return View(model);
        }

        // Visual to the website
        public IActionResult Create() //id is the username
        {
            return View();
        }

        //SQL database stuff
        [HttpPost]
        public async Task<IActionResult> AddThread(Thread model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var thread = BuildThread(model, user);

            _context.Add(thread);
            //TODO User score HERE
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", thread.ID);
        }

        private Thread BuildThread(Thread model, LoginUser user)
        {
            return new Thread
            {
                Title = model.Title,
                CreateDate = DateTime.Now,
                Description = model.Description,
                ID = model.ID,
                Image = model.Image,
                UserID = user.Id,
                Votes = model.Votes
            };
        }
    }
}
