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
    public class ChannelController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly ChannelContext _context;

        public ChannelController(UserManager<LoginUser> userManager, ChannelContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        public async Task<IActionResult> Main(int? id)
        {
            var user = await _context.Channel
                .FirstOrDefaultAsync(table => table.Id == id);
            if (user == null) {}
            var username = _userManager.GetUserName(User);
            ViewData["username"] = username;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}