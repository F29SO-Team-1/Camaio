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

        public IActionResult Index()
        {
            var user = _userManager.GetUserId(User);
            var userChannels = _context.ChannelMembers
                .Where(table => table.UserId == user)
                .Join(
                    _context.Channel,
                    channelMembers => channelMembers.ChannelId,
                    channel => channel.Id,
                    (channelMember, channel) => channel.Title
                )
                .ToList();
            return View(userChannels);
        }
        
        public async Task<IActionResult> Main(string id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            if (channel == null) 
            {
                return NotFound();
            }
            ViewData["owner"] = false;
            var userId = _userManager.GetUserId(User);
            var channelMember = await _context.ChannelMembers
                .Where(table => table.ChannelId == channel.Id)
                .Where(table => table.UserId == userId)
                .FirstOrDefaultAsync();
            if (channelMember == null) 
            {
                ViewData["member"] = false;
            } else 
            {
                ViewData["member"] = true;
            }
            if (channel.Creator == _userManager.GetUserName(User))
            {
                ViewData["owner"] = true;
            }
            return View(channel);
        }
        public async Task<IActionResult> JoinChannel(string id)
        {
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            var userId = _userManager.GetUserId(User);
            var channelMember = new ChannelMember 
            {
                ChannelId = channel.Id,
                UserId = userId
            };
            _context.Add(channelMember);
            _context.SaveChanges();
            return RedirectToAction("Main", "Channel", new { id = channel.Title} );
        }
        public async Task<IActionResult> LeaveChannel(string id)
        {
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            var userId = _userManager.GetUserId(User);
            var channelMember = await _context.ChannelMembers
                .Where(table => table.ChannelId == channel.Id)
                .Where(table => table.UserId == userId)
                .FirstOrDefaultAsync();
            _context.ChannelMembers.Remove(channelMember);
            _context.SaveChanges();
            return RedirectToAction("Main", "Channel", new { id = channel.Title} );
        }

        public async Task<IActionResult> DeleteChannel(string id)
        {
            var channel = await _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            var userId = _userManager.GetUserId(User);

            return NotFound();
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