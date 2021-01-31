using Login.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Login.Models.Threadl;

namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThread _threadService;
        public ThreadController(IThread thread)
        {
            _threadService = thread;
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
    }
}
