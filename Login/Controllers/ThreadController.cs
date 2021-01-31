using Login.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        // GET: Threads/Create
        public IActionResult Create()
        {
            return View();
        }

        
    }
}
