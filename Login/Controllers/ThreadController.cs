using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.Threadl;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThread _service;
        private readonly IConfiguration _configuration;
        private readonly UserManager<LoginUser> _userManager;
        private readonly IUpload _uploadService;

        public ThreadController(IThread thread,
            IConfiguration configuration,
            UserManager<LoginUser> userManager,
            IUpload uploadService)
        {
            _service = thread;
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
            //make a list of users that liked the thread
            var listOfLikes = _service.ListOfLikes(id);

            //make a view model for the thread
            var model = new ThreadModel
            {
                Id = thread.ID,
                AuthorId = thread.UserID,
                AuthorUserName = thread.UserName,
                Created = thread.CreateDate,
                Description = thread.Description,
                Picture = thread.Image,
                Title = thread.Title,
                Rating = thread.Votes,
                LikedBy = listOfLikes
            };
            return View(model);
        }

        [Route("Score/Threads")]
        public IActionResult Scores()
        {
            var threadModel = _service.GetAll().Select(threads => new ThreadModel
            {
                Title = threads.Title,
                Rating = threads.Votes,
                Created = threads.CreateDate,
                Picture = threads.Image,
                Id = threads.ID
            })
                .OrderByDescending(x => x.Rating)
                .ToList();

            var threadList = new ThreadList { ThreadLists = threadModel };
            return View(threadList);
        }

        //takes in a ajax call from the view, returns a JSON back to the view, the like btn
        [Authorize]
        public async Task<IActionResult> RatingIncrement([FromBody] int? id)
        {
            var userId = _userManager.GetUserId(User);  //gets the usersId
            var wholeThread = _service.GetById(id);
            var thread = _service.GetById(id).Votes; // gets the votes from the current thread
            if (id == null) NotFound();
            //check if the user already pressed the btn
            if (_service.CheckAreadyLiked(wholeThread, userId) == true)
            {
                return Json(thread);
            }
            else
            {
                //add the user that pressed the button to the list of liked on the thread
                await _service.AddUserToLikeList(id, userId);
                await _service.IncrementRating(id); //this increments the vote
                return Json(_service.GetById(id).Votes);    //makes a json with the amount of votes that are currently in the database
            }
            
        }
        [Authorize]
        public async Task<IActionResult> RatingDecrease([FromBody]int? id)
        {
            var userId = _userManager.GetUserId(User);  //gets the usersId
            var wholeThread = _service.GetById(id);
            var thread = _service.GetById(id).Votes; // gets the votes from the current thread
            if (id == null) NotFound();
            if (_service.CheckAreadyLiked(wholeThread, userId) == true)
            {
                //decrease the rating
                await _service.DecreaseRating(id);
                //remove the user from the table
                await _service.RemoveUserFromLikeList(id, userId);
                //show the decerase 
                return Json(_service.GetById(id).Votes);
            }
            else
            {
                return Json(thread);    //makes a json with the amount of votes that are currently in the database
            }
        }

        // Visual to the website
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        //Get request, View
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
            var thread = _service.Create(model, user);  //creates the thread
            var threadId = thread.Result.ID;    //gets the Threads id
            await UploadThreadImage(file, threadId);    //uploads the threadImage
            return RedirectToAction("Index", "Thread", new { @id = threadId });    //shows the thread that was created
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
        public async Task<IActionResult> DeleteThread(int? id)
        {
            if (id == null) return NotFound();
            await _service.Delete(id);
            return RedirectToAction("Index", "Home");
        }
    }
}
