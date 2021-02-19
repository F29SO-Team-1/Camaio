using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models.ApplicationUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace Login.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<LoginUser> _userManager;
        private readonly IApplicationUsers _userService;
        public RoleController(RoleManager<IdentityRole> roleManager, 
            IApplicationUsers userService , 
            UserManager<LoginUser> userManager)
        {
            _roleManager = roleManager;
            _userService = userService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    return NotFound();
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index");
        }

        public IActionResult AssignUser()
        { 
            var userModel = _userService.GetAll().Select(user => new ProfileModel
            {
                Username = user.UserName,
                UserRating = user.Ratting,
                Warnings = user.AccountWarnings
            })
                .ToList();

            var userList = new ProfileModelList { ProfileList = userModel };

            return View(userList);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin()
        {
            await _userManager.AddToRoleAsync(await _userManager.GetUserAsync(User), "Admin");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeMod(string userId)
        {
            LoginUser user =  _userService.GetById(userId);
            await _userManager.AddToRoleAsync(user, "Mod");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeUser(string userId)
        {
            //need to check if they are not a different role already
            LoginUser user = _userService.GetById(userId);
            await _userManager.AddToRoleAsync(user, "User");
            return RedirectToAction("Index");
        }

    }
}
