using Login.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Login.Areas.Identity.Data;
using Login.Data;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly UserContext _context;

        public ProfileController(UserManager<LoginUser> userManager, UserContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var username = _userManager.GetUserName(User);
            ViewData["username"] = username;
            return View();
        }
        public async Task<IActionResult> Users(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(table => table.UserName == id);
            if (user == null)
            {
                return NotFound();
            }
            var username = _userManager.GetUserName(User);
            ViewData["username"] = id;
            if (username == id) 
            {
                return View("../Profile/Index");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}