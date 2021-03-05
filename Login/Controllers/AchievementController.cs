using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Login.Controllers
{
    public class AchievementController : Controller
    {
        private readonly IAchievement _service;
        private readonly IApplicationUsers _userService;
        private readonly UserManager<LoginUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUpload _uploadService;
        public AchievementController(IAchievement service, 
            IApplicationUsers userService, 
            UserManager<LoginUser> userManager,
            IConfiguration configuration,
            IUpload uploadService)
        {
            _service = service;
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [Route("{username}/Achievements")]
        public async Task<IActionResult> IndexAsync(string username)
        {
            LoginUser user = _userService.GetByUserName(username);
            int? usersAch = _service.GetUsersAchievement(user).Count();     //user's Achievements
            int totalAmountOfAch = _service.GetAllAchievements().Count();   //total amount of Achievements there is 
            if (usersAch == null || totalAmountOfAch == 0) return NotFound();
            //checks the number of Achievements compared to the number of users Achievements
            await _service.AssignAchievementsToUser(user);
            



            //build model
            var model = _service.GetUsersAchievement(user).Select(achiev => new AchievementModel
            {
                Picture = achiev.Achievement.Picture,
                Name = achiev.Achievement.Name,
                Description = achiev.Achievement.Description,
                Progress = achiev.UsersProgress,
                ProgressLimit = achiev.MaxProgress,
                CompletedTime = achiev.CompletedTime,
                Completed = achiev.Completed
            });

            var usersAchievementList = new AchievementModelList { AchievementLists = model };
            return View(usersAchievementList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateAchievement(Achievement model, IFormFile file)
        {
            var ach = await _service.MakeAchievement(model);
            var achId = ach.Id;
            await UploadThreadImage(file, achId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task UploadThreadImage(IFormFile file, int id)
        {
            var ach = _service.GetById(id);
            //connect to azure account container
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            //get the blog container
            var container = _uploadService.GetBlobContainer(connectionString, "achievement-images");
            //parse the context disposition response header
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            //grab the filename
            var filename = contentDisposition.FileName.Trim('"');
            //get a refrence to a block blob
            var blockBlob = container.GetBlockBlobReference(filename);
            //On that block blob, Upload our file <-- file uploaded to the cloud
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            //set the thread image to the URI
            await _service.UploadPicture(ach.Id, blockBlob.Uri);
        }
    }
}
