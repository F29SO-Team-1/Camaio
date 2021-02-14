using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.ApplicationUser;
using Login.Models.Threadl;
using Login.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Login.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly IApplicationUsers _userService;
        private readonly IConfiguration _configuration;
        private readonly IUpload _uploadService;
        private readonly IThread _threadService;
        private readonly IChannel _channelSerivce;

        public ProfileController(
            UserManager<LoginUser> userManager,
            IApplicationUsers userService,
            IUpload uploadService,
            IConfiguration configuration,
            IThread threadService,
            IChannel channelSerivce
            )
        {
            _userManager = userManager;
            _userService = userService;
            _configuration = configuration;
            _uploadService = uploadService;
            _threadService = threadService;
            _channelSerivce = channelSerivce;
        }

        [Route("Profile/{username}")]
        public IActionResult Index(string username)
        {
            var user = _userService.GetByUserName(username);
            //want a list of threads, tick
            // threads will only display if you press your username when logged in button other wise it will not display the users threads
            var threads = BuildThreadList(username);
            //want a list of channels that the user is part of, tick
            var channels = BuildChannelsList(username);
            //calc the users Ratting
            var ratting = _userService.GetRatting(username, threads);

            var model = new ProfileModel()
            {
                Username = user.UserName,
                UserId = user.Id,
                UserRating = ratting,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemmberSince = user.MemberSince,
                Threads = threads,
                Channels = channels
                
            };
            return View(model);
        }
        
        [Route("Score/Users")]
        public IActionResult Scores()
        {
            var userModel = _userService.GetAll().Select(user => new ProfileModel
            {
                ProfileImageUrl = user.ProfileImageUrl,
                Username = user.UserName,
                UserRating = user.Ratting
            })
                .OrderByDescending(x => x.UserRating)
                .ToList(); ;

            var userList = new ProfileModelList { ProfileList = userModel };

            return View(userList);
        }

        private IEnumerable<ChannelModel> BuildChannelsList(string username)
        {
            return _channelSerivce.UserChannel(username).Select(c => new ChannelModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreationDate = c.CreationDate
            });
        }

        //makes the model to me passed in the view
        private IEnumerable<ThreadModel> BuildThreadList(string userName)
        {
            return _threadService.UserThreads(userName).Select(threads => new ThreadModel
            {
                Title = threads.Title,
                Description = threads.Description,
                Created = threads.CreateDate,
                Picture = threads.Image,
                AuthorUserName = threads.UserName,
                Rating = threads.Votes,
                Id = threads.ID
            });
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userName = _userManager.GetUserName(User);
            if (file == null) return RedirectToAction("Index", "Profile", new { username = userName });
            //connect to azure account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //get the blog container
            var container = _uploadService.GetBlobContainer(connectionString, "profile-images");
            //parse the context disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            //grab the filename
            var filename = contentDisposition.FileName.Trim('"');
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(filename);
            //On that block blob, Upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            //set the users profileimage to the URI
            await _userService.SetProfileImage(userName, blockBlob.Uri);
            //redirects to the users's profile page
            return RedirectToAction("Index", "Profile", new { username = userName });
        }
    }
}

