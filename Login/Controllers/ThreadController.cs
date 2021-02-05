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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Login.Service;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThread _service;
        private readonly ThreadContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<LoginUser> _userManager;
        private readonly IUpload _uploadService;

        public ThreadController(IThread thread, 
            ThreadContext context, 
            IConfiguration configuration, 
            UserManager<LoginUser> userManager, 
            IUpload uploadService)
        {
            _service = thread;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _uploadService = uploadService;
        }

        [Route("Thread/{id?}")]
        //To view ONE thread whatever
        public IActionResult Index(int? id)
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
                Title = thread.Title,
                Rating = thread.Votes
            };

            return View(model);
        }


        /*public IActionResult RatingIncrement(int? id)
        {
            _service.IncrementRating(id);
            //return View();
        }*/

        // Visual to the website
        [Authorize]
        public IActionResult Create() //id is the username
        {
            return View();
        }

        //Get request, View
        public IActionResult Edit(int? threadId)
        {
            if (threadId == null) return NotFound();

            var thread = _service.GetById(threadId);
            if (thread == null) return NotFound();

            return View(thread);
        }

        // does the post to the db
        [HttpPost]
        public async Task<IActionResult> Edit(Thread thread)
        {
            if (thread == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Edit(thread);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.ThreadExists(thread.ID)) return NotFound(); 
                    else throw;
                }
                return RedirectToAction("Index", "Thread", new { @id = thread.ID });
            }

            return View(thread);
        }

        //SQL database stuff
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddThread(Thread model, IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var thread = BuildThread(model, user);

            _context.Add(thread);

            await _context.SaveChangesAsync();

            await UploadTreadImage(file, thread.ID);
            //TODO User score HERE

            return RedirectToAction("Index", "Thread", new { @id = thread.ID });
        }

        private Thread BuildThread(Thread model, LoginUser user)
        {
            return new Thread
            {
                Title = model.Title,
                CreateDate = DateTime.Now,
                Description = model.Description,
                ID = model.ID,
                UserID = user.Id,
                Votes = model.Votes
            };
        }

        [HttpPost]
        public async Task<IActionResult> UploadTreadImage(IFormFile file, int id)
        {
            var thread = _service.GetById(id);
            //connect to azure account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //get the blog container
            var container = _uploadService.GetBlobContainer(connectionString, "thread-storage");
            //parse the context disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            //grab the filename
            var filename = contentDisposition.FileName.Trim('"');
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(filename);
            //On that block blob, Upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            //set the thread image to the URI
            await _service.UploadPicture(thread.ID, blockBlob.Uri);

            return RedirectToAction("Index", "Profile");
        }
    }
}
