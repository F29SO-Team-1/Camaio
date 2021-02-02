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
        
        public async Task<IActionResult> Main(string id)
        {
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            if (channel == null) 
            {
                return NotFound();
            }
            return View(channel);
        }

        public IActionResult Create()
        {
            ViewData["Exists"] = false;
            return View();
        }

        public async Task<IActionResult> CreateChannel(string title, string description)
        {
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == title);
            if (channel == null) 
            {
                channel = new Channel()
                {
                    Creator = _userManager.GetUserName(User),
                    Title = title,
                    Description = description,
                    Public = true,
                    VisibleToGuests = true,
                    MembersCanPost = true,
                    CreationDate = DateTime.Now
                };
                _context.Add(channel);
                _context.SaveChanges();
                
                var ChannelMember = new ChannelMember() 
                { 
                    ChannelId = channel.Id, 
                    UserId = _userManager.GetUserId(User)
                };
                _context.Add(ChannelMember);
                await _context.SaveChangesAsync();
                ViewData["Channel"] = title;
                return View();

            } else
            {
                ViewData["Exists"] = true;
                return View("Create");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}