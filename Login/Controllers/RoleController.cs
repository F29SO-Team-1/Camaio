using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models.ApplicationUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IActionResult> IndexAsync()
        {
            var roles = _roleManager.Roles.ToList();
            var adminCheck = await _roleManager.RoleExistsAsync("Admin");
            var modCheck = await _roleManager.RoleExistsAsync("Mod");
            if (!adminCheck && !modCheck)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Mod"));
            }
            
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
            //var userRoles = GetUsersRole();
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

        private async Task<IList<string>> GetUsersRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return new List<string>(await _userManager.GetRolesAsync(user));
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
            //unasign from others roles and then asign to new role
            LoginUser user =  _userService.GetById(userId);

            var userRole =  await _userManager.GetRolesAsync(user);
            foreach (var role in userRole)
            {
                if (role == "Mod") return RedirectToAction("Index");
            }

            await _userManager.AddToRoleAsync(user, "Mod");
            return RedirectToAction("Index");
        }

    }
}
