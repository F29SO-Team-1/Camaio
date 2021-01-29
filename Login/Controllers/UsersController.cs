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

namespace Login.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        
        internal AppDb Db { get; }

        public UsersController(ILogger<UsersController> logger, UserManager<LoginUser> userManager, AppDb db)
        {
            _logger = logger;
            _userManager = userManager;
            Db = db;
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await Db.Connection.OpenAsync();
            var query = new UserModel.UserQuery(Db);
            var result = await query.FindUser(id);
            if (result is null) return NotFound();

            var username = _userManager.GetUserName(User);
            if (id==username) {
                ViewData["ownership"] = true;
            } else {
                ViewData["ownership"] = false;
            }
            ViewData["username"] = id;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
