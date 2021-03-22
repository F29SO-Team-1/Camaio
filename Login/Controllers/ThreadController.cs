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
        private readonly IChannel _channelService;
        private readonly IAlbum _albumService;
        private readonly IApplicationUsers _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ThreadController(IThread thread,
            IConfiguration configuration,
            UserManager<LoginUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUpload uploadService,
            IApplicationUsers userService,
            IAlbum albumService,
            IChannel channelService)
        {
            _service = thread;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _uploadService = uploadService;
            _userService = userService;
            _albumService = albumService;
            _channelService = channelService;
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
            //get the list of reports
            var numberOfReports = _service.ListOfReports(id).Count();

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
                Rating = listOfLikes.Count(),
                LikedBy = listOfLikes,
                NoReports = numberOfReports
            };
            return View(model);
        }

        //only allow modertators and admins to access the page
        [Route("Reported")]
        [Authorize(Roles = "Admin, Mod")]
        public IActionResult Reported()
        {
            var threadModel = _service.GetAll().Select(threads => new ThreadModel
            {
                Id = threads.ID,
                Title = threads.Title,
                Created = threads.CreateDate,
                Description = threads.Description,
                Rating = threads.Votes,
                AuthorUserName = threads.UserName,
                NoReports = threads.NoReports
            })
                .Where(x => x.NoReports >= 1)
                .OrderByDescending(x => x.NoReports)
                .ToList();

            var threadList = new ThreadList { ThreadLists = threadModel };
            return View(threadList);
        }

        //Clears the reports that the thread had/has
        public async Task<IActionResult> ResetReports(int? threadsId)
        {
            await _service.ResetReports(threadsId);
            return RedirectToAction("Reported", "Thread");
        }

        //gives a user a warning and flaggs the thread/post
        public async Task<IActionResult> FlagPost(int? threadsId)
        {
            //flags a thread
            await _service.FlagThread(threadsId);
            return RedirectToAction("Reported", "Thread");
        }

        //Deletes the thread and gives a warning 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDelete(int? threadsId)
        {
            Thread t = _service.GetById(threadsId);
            var user = t.UserID;
            //delete from reports first
            await ResetReports(threadsId);
            //deletes the thread
            await _service.Delete(threadsId);
            //Gives a user a warining
            await _userService.GiveUserWarning(user);

            return RedirectToAction("Reported", "Thread");
        }

        //allows a user to report a thread
        [Authorize]
        public async Task<IActionResult> Report(int? threadId)
        {
            var username = _userManager.GetUserName(User);
            await _service.Report(threadId, username);
            return RedirectToAction("Index", "Thread", new { @id = threadId });
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
            //make a list of users that liked the thread
            var listOfLikes = _service.ListOfLikes(id);
            if (id == null) NotFound();
            //check if the user already pressed the btn
            if (_service.CheckAreadyLiked(wholeThread, userId) == true)
            {
                return Json(listOfLikes.Count());
            }
            else
            {
                //add the user that pressed the button to the list of liked on the thread
                await _service.AddUserToLikeList(id, userId);
                wholeThread.Votes = listOfLikes.Count();
                return Json(listOfLikes.Count());    //makes a json with the amount of votes that are currently in the database
            }
            
        }
        
        [Authorize]
        public async Task<IActionResult> RatingDecrease([FromBody]int? id)
        {
            var userId = _userManager.GetUserId(User);  //gets the usersId
            var wholeThread = _service.GetById(id);
            var listOfLikes = _service.ListOfLikes(id);
            if (id == null) NotFound();
            if (_service.CheckAreadyLiked(wholeThread, userId) == true)
            {
                //remove the user from the table
                await _service.RemoveUserFromLikeList(id, userId);
                //show the decerase 
                return Json(listOfLikes.Count());
            }
            else
            {
                return Json(listOfLikes.Count());    //makes a json with the amount of votes that are currently in the database
            }
        }

        // Visual to the website
        [Authorize]
        public IActionResult Create(int albumId)
        {
            if(albumId != 1) 
            {
                var album = _albumService.GetAlbum(albumId);
                if (album == null) return NotFound();
                var channel = _channelService.GetChannel(album.ChannelId).Result;
                var user = _userManager.GetUserAsync(User).Result;
                var channelMember = _channelService.GetChannelMember(user, channel).Result;
                if (channelMember == null) return NotFound();
                if (!(album.MembersCanPost || user.Id == channel.CreatorId)) return NotFound();
            }
            ViewData["albumId"] = albumId;
            return View();
        }

        //Get request, View
        public IActionResult Edit(int? threadId)
        {
            var userName = _userManager.GetUserName(User); //gets the usersName
            if (threadId == null) return NotFound();    //check if the threadId is passed as a param
            var thread = _service.GetById(threadId);    //gets the thread Id
            if (thread == null) return NotFound();      //check if the thread is a real thread
            if (thread.UserName != userName) return NotFound(); //checks if the person accessing the thread is the owner

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
        public async Task<IActionResult> AddThread(int albumId, Thread model, IFormFile file)
        {
            var userId = _userManager.GetUserId(User);  //gets the usersId
            var user = await _userManager.FindByIdAsync(userId);    //gets the userName
            var thread = _service.Create(model, user, albumId);  //creates the thread
            var threadId = thread.Result.ID;    //gets the Threads id
            await UploadThreadImage(file, threadId);    //uploads the threadImage
            return RedirectToAction("Index", "Thread", new { @id = threadId });    //shows the thread that was created
        }

        //Uploads the Image to the Azure blob container
        [HttpPost]
        public async Task<IActionResult> UploadThreadImage(IFormFile file, int id)
        {
            var thread = _service.GetById(id);
            var userName = thread.UserName;
            var date = thread.CreateDate.Ticks;
            //connect to azure account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //get the blog container
            var container = _uploadService.GetBlobContainer(connectionString, "thread-storage");
            //parse the context disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            //grab the filename
            var filename = contentDisposition.FileName.Trim('"');
            var uniqueFileName = filename + userName + date;
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(uniqueFileName);
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
            if (thread == null) return NotFound();
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
