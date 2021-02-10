using Login.Data;
using Microsoft.AspNetCore.Mvc;
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
        //To view ONE thread
        public IActionResult Index(int? id)
        {
            //get the id of the thread
            var thread = _service.GetById(id);
            if (thread == null) return NotFound(); //if the thread number does not exist then not found
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
                Rating = thread.Votes,
                AuthorUserName = thread.UserName
            };
            return View(model);
        }

        //takes in a ajax call from the view, returns a JSON back to the view, the like btn
        public async Task<IActionResult> RatingIncrement([FromBody]int? id)
        {
            if (id == null) NotFound();
            await _service.IncrementRating(id); //this increments the vote
            var thread = _service.GetById(id).Votes; // gets the votes from the current thread
            return Json(thread);    //makes a json with the amount of votes that are currently in the database
        }

        // Visual to the website
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //Get request, View
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? threadId)
        {
            var userName = _userManager.GetUserName(User); //gets the usersName
            if (threadId == null) return NotFound();    //check if the threadId is passed as a param
            var thread = _service.GetById(threadId);    //gets the thread Id
            if (thread == null) return NotFound();      //check if the thread is a real thread
            if (thread.UserName != userName) return NotFound();

            return View(thread);
        }

        // does the post to the db
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            var userId = _userManager.GetUserId(User);  //gets the usersId
            var user = await _userManager.FindByIdAsync(userId);    //gets the userName

            var thread = BuildThread(model, user);  //builds the thread

            _context.Add(thread);   //adds it to the Db

            await _context.SaveChangesAsync();  //saves the changes

            await UploadThreadImage(file, thread.ID);    //uploads the threadImage

            return RedirectToAction("Index", "Thread", new { @id = thread.ID });    //shows the thread that was created
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
                Votes = model.Votes,
                UserName = user.UserName
            };
        }

        //Uploads the Image to the Azure blob container
        [HttpPost]
        public async Task<IActionResult> UploadThreadImage(IFormFile file, int id)
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

        //functions for displaying the Delete thread page
        public IActionResult Delete(int? threadId)
        {
            var userName = _userManager.GetUserName(User);
            var thread = _service.GetById(threadId);
            if (thread.UserName != userName) return NotFound();
            return View(thread);
        }
        
        //action of deleting the thread
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<Thread>> DeleteThread(int id)
        {
            var todoItem = await _context.Threads.FindAsync(id);
            if (todoItem == null) return NotFound();
            _context.Threads.Remove(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
