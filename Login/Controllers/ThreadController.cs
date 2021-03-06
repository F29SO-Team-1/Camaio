using Login.Areas.Identity.Data;
using Login.Data;
using Login.Data.Interfaces;
using Login.Models;
using Login.Models.Threadl;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Login.Controllers
{
    public class ThreadController : Controller
    {
        //azure vision api key and endpoint
        static readonly string subscriptionKey = "5d7d56109a794e2b9532bdde2185755d";
        static readonly string endpoint = "https://camaioai.cognitiveservices.azure.com/";

        //injections
        private readonly IThread _service;
        private readonly IConfiguration _configuration;
        private readonly UserManager<LoginUser> _userManager;
        private readonly IUpload _uploadService;
        private readonly IChannel _channelService;
        private readonly IAlbum _albumService;
        private readonly IApplicationUsers _userService;
        private readonly IVision _visionService;


        public ThreadController(IThread thread,
            IConfiguration configuration,
            UserManager<LoginUser> userManager,
            IUpload uploadService,
            IApplicationUsers userService,
            IVision visionService,
            IAlbum albumService,
            IChannel channelService)
        {
            _service = thread;
            _configuration = configuration;
            _userManager = userManager;
            _uploadService = uploadService;
            _userService = userService;
            _visionService = visionService;
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
            var tags = _service.GetThreadTags(thread);

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
                NoReports = numberOfReports,
                Tags = tags,
                Lat = thread.Lat,
                Lng = thread.Lng
            };
            return View(model);
        }

        //only allow modertators and admins to access the page
        /*
         *      This is where you can change the number of reposts needed to be displayed on the report page
         */
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

        /*
         *  Adding AI HERE 
         */

        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        private async Task<bool> AI(int? id)
        {
            var thread = _service.GetById(id);

            string imageUri = thread.Image;

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            var r = await _visionService.AnalyzeImageUrl(client, imageUri);

            return _visionService.Description(r);
        }

        //allows a user to report a thread
        [Authorize]
        public async Task<IActionResult> Report(int? threadId)
        {
            var username = _userManager.GetUserName(User);
            bool hasHumanParts = await AI(threadId);
            //add AI HERE for AI (need to call flag post if the AI description has a tag of any human parts)
            if (hasHumanParts)
            {
                await FlagPost(threadId);
            }
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
                Id = threads.ID,
                Flagged = threads.Flagged,
                AlbumId = threads.AlbumId
            })
                .OrderByDescending(x => x.Rating)
                .ToList();

            var threadList = new ThreadList { ThreadLists = threadModel };
            return View(threadList);
        }

        //takes in a ajax call from the view, returns a JSON back to the view, the like btn
        [Authorize]
        public async Task<JsonResult> RatingIncrement([FromBody] int? id)
        {
            var userId = _userManager.GetUserId(User);  //gets the usersId
            if (id == null) return Json("Error");
            var wholeThread = _service.GetById(id);
            //make a list of users that liked the thread
            var listOfLikes = _service.ListOfLikes(id);
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
        public async Task<JsonResult> RatingDecrease([FromBody] int? id)
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
            if (albumId != 1)
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
        public async Task<IActionResult> Edit(int? threadId)
        {
            var userName = _userManager.GetUserName(User); //gets the usersName
            LoginUser user = _userService.GetByUserName(userName);
            var userRole = await _userManager.GetRolesAsync(user);
            if (threadId == null) return NotFound();    //check if the threadId is passed as a param
            var thread = _service.GetById(threadId);    //gets the thread Id
            if (thread == null) return NotFound();      //check if the thread is a real thread
            //checks if the person accessing the thread is the owner
            if (userRole.Contains("Admin") || thread.UserName == userName)
            {
                var tags = _service.GetThreadTags(thread);
                var tagline = "";
                foreach (var tag in tags)
                {
                    tagline += ",";
                    tagline += tag.Name;
                }
                if (tagline.Length != 0) tagline = tagline.Substring(1);
                ViewData["Tags"] = tagline;

                return View(thread);
            }
            if (thread.UserName != userName) return NotFound();
            return RedirectToAction("Index", "Thread", new { @id = threadId });
        }

        // does the post to the db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Thread thread, string tags)
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
                _service.ChangeTags(thread, tags);
                return RedirectToAction("Index", "Thread", new { @id = thread.ID });
            }

            return View(thread);
        }

        //SQL database stuff
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddThread(int albumId, Thread model, IFormFile file, string tags)
        {
            //the type of files that the system ONLY accepts
            if (file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            {
                var userId = _userManager.GetUserId(User);  //gets the usersId
                var user = await _userManager.FindByIdAsync(userId);    //gets the userName
                var thread = _service.Create(model, user, albumId);  //creates the thread
                _service.ChangeTags(thread.Result, tags);
                var threadId = thread.Result.ID;    //gets the Threads id
                await _service.AssignCords(file, threadId);    //assignes the cords from the picture
                await UploadThreadImage(file, threadId);    //uploads the threadImage

                return RedirectToAction("Index", "Thread", new { @id = threadId });    //shows the thread that was created
            }
            return View("../Shared/Error");
        }

        //Uploads the Image to the Azure blob container
        [HttpPost]
        public async Task UploadThreadImage(IFormFile file, int id)
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
            var uniqueFileName = userName + date + filename;
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(uniqueFileName);
            //On that block blob, Upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            //set the thread image to the URI
            await _service.UploadPicture(thread.ID, blockBlob.Uri);
        }


        //functions for displaying the Delete thread page
        public IActionResult Delete(int? threadId)
        {
            var userName = _userManager.GetUserName(User);
            var thread = _service.GetById(threadId);
            if (thread == null) return NotFound();
            var creator = _service.GetChannelCreator(thread);
            if (thread.UserName != userName && creator != userName) return NotFound();
            return View(thread);
        }

        //action of deleting the thread
        public async Task<IActionResult> DeleteThread(int? id)
        {
            if (id == null) return NotFound();
            var thread = _service.GetById(id);
            await _service.Delete(id);
            return RedirectToAction("Index", "Profile", new { username = thread.UserName });
        }
    }
}
