using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.ApplicationUser;
using Login.Models.Threadl;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicationUsers _service;
        private readonly IConfiguration _configuration;
        private readonly IUpload _uploadService;
        private readonly IThread _threadService;
        private readonly IChannel _channelSerivce;
        private readonly IAchievement _achievementService;

        public ProfileController(
            UserManager<LoginUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IApplicationUsers userService,
            IUpload uploadService,
            IConfiguration configuration,
            IThread threadService,
            IChannel channelSerivce,
            IAchievement achievementService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _service = userService;
            _configuration = configuration;
            _uploadService = uploadService;
            _threadService = threadService;
            _channelSerivce = channelSerivce;
            _achievementService = achievementService;
        }

        [Route("Profile/{username}")]
        public async Task<IActionResult> Index(string username)
        {
            if (!_service.IfUserExists(username)) return NotFound();
            var user = _service.GetByUserName(username);
            //want a list of threads, tick
            // threads will only display if you press your username when logged in button other wise it will not display the users threads
            var threads = BuildThreadList(username);
            //want a list of channels that the user is part of, tick
            var channels = BuildChannelsList(username);
            //calc the users Ratting
            var ratting = _service.GetRatting(username, threads);
            //list of all the users that the user follows
            var listOfFollower = _service.UsersFollowers(user);
            //user roles 
            var userRoles = _userManager.GetRolesAsync(user);
            //gives the inital achievements to the user 
            await _achievementService.AssignAchievementsToUser(user);

            /*
             * Achievements HERE
             */
            //makes sure that the user is the user
            if (username == user.UserName) await GiveUserLoginAch(user);
            if(listOfFollower.Count() != _achievementService.FollowAchievementProgress(user) 
                && _achievementService.GetUsersAchievement(user).Count() != 0) GiveTenFollowAch(user);

            //build model
            var model = new ProfileModel()
            {
                Username = user.UserName,
                UserId = user.Id,
                UserRating = ratting,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemmberSince = user.MemberSince,
                Threads = threads,
                Channels = channels,
                UsersFollowed = listOfFollower,
                Warnings = user.AccountWarnings,
                Roles = userRoles

            };
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Follow(string id)
        {
            //user that presses the button
            var user = _userManager.GetUserName(User);
            await _service.Follows(user, id);
            return RedirectToAction(id);
        }


        //scoreboard of all the users
        [Route("Score/Users")]
        public IActionResult Scores()
        {
            var userModel = _service.GetAll().Select(user => new ProfileModel
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
            return _threadService.UserThreadsWithoutAlbum(userName).Select(threads => new ThreadModel
            {
                Title = threads.Title,
                Description = threads.Description,
                Created = threads.CreateDate,
                Picture = threads.Image,
                AuthorUserName = threads.UserName,
                Rating = threads.Votes,
                Id = threads.ID,
                Flagged = threads.Flagged
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
            await _service.SetProfileImage(userName, blockBlob.Uri);
            //redirects to the users's profile page
            return RedirectToAction("Index", "Profile", new { username = userName });
        }

        private async Task GiveUserLoginAch(LoginUser user)
        {
            if (_achievementService.GetUsersAchievement(user).Count() == 0) return;
            
            // if the user has the following achievement then do the following else ignore
            if (!_achievementService.CheckProgression(user, 1))
            {
                await _achievementService.GiveFirstLoginAchievement(user);
            }
        }

        private void GiveTenFollowAch(LoginUser user)
        {
            int numberOfFollowing = _service.UsersFollowers(user).Count();

            if (_achievementService.CheckProgression(user, 3))
            {
                return;
            }
            else
            {
                if (numberOfFollowing >= 10)
                {
                    _achievementService.GiveTenAchievement(user);
                }
                _achievementService.IncrementAchievementProgress(user, 3);
            }
        }
    }
}

