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
        public IActionResult List()
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
        public async Task<IActionResult> AddThread(Thread model, IFormFile file)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var thread = BuildThread(model, user, file);

            _context.Add(thread);

            await _context.SaveChangesAsync();

            UploadTreadImage(file, thread.ID);
            //TODO User score HERE



            return RedirectToAction("Index", "Thread", new { thread = thread.ID });
        }

        private Thread BuildThread(Thread model, LoginUser user, IFormFile file)
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

        public void UploadTreadImage(IFormFile file, int id)
        {
            var thread = _service.GetById(id);
            //connect to azure account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //get the blog container
            var container = _uploadService.GetBlobContainer(connectionString);
            //parse the context disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            //grab the filename
            var filename = contentDisposition.FileName.Trim('"');
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(filename);
            //On that block blob, Upload our file <-- file uploaded to the cloud
            blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            //set the thread image to the URI
            _service.UploadPicture(thread.ID, blockBlob.Uri);
        }
    }
}
