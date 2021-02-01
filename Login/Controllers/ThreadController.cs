using Login.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Login.Models.Threadl;
using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Login.Service;
using System.Net.Http.Headers;

namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThread _threadService;
        private readonly ThreadContext _context;
        private readonly IConfiguration _configuration;

        public ThreadController(IThread thread, ThreadContext context, IConfiguration configuration)
        {
            _threadService = thread;
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var threads = _threadService.GetAll().Select(t => new ThreadListingModel
            {
                Id = t.ID,
                Title = t.Title,
                Description = t.Description
            });

            var model = new ThreadIndexModel
            {
                ThreadList = threads
            };

            return View(model);
        }

        // GET: Threads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Threads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID,Title,Location,Image,Description,CreateDate,Votes")] Thread thread)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thread);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thread);
        }

    }
}
