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
    public class ChannelController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly IChannel _service;

        public ChannelController(UserManager<LoginUser> userManager, IChannel service)
        {
            _userManager = userManager;
            _service = service;
        }

        public IActionResult Index()
        {
            var user = _userManager.GetUserName(User);
            var userChannels = _service.GetChannels(user);
            return View(userChannels);
        }
        
        public IActionResult Main(string id)
        {
            if (id == null) 
            {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            if (channel == null) 
            {
                return NotFound();
            }
            ViewData["owner"] = false;
            var userName = _userManager.GetUserName(User);
            var channelMember = _service.GetChannelMember(userName, channel).Result;
            if (channelMember == null) 
            {
                ViewData["member"] = false;
            } else 
            {
                ViewData["member"] = true;
            }
            if (channel.Creator == userName)
            {
                ViewData["owner"] = true;
            }
            return View(channel);
        }
        public IActionResult JoinChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            var userName = _userManager.GetUserName(User);
            _service.AddMember(channel, userName);
            return RedirectToAction("Main", "Channel", new { id = channel.Title} );
        }
        public IActionResult LeaveChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            var userName = _userManager.GetUserName(User);
            var channelMember = _service.GetChannelMember(userName, channel).Result;
            _service.RemoveMember(channelMember);
            return RedirectToAction("Main", "Channel", new { id = channel.Title} );
        }

        public async Task<IActionResult> Delete(string id)
        {   var channel = _service.GetChannel(id).Result;
            var user = _userManager.GetUserName(User);
            if (user == channel.Creator) 
            {
                await _service.DeleteChannel(channel);
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public IActionResult RemoveMembers(string id)
        {   var channel = _service.GetChannel(id).Result;
            var user = _userManager.GetUserName(User);
            if (user == channel.Creator) 
            {
                var channelUsers = _service.GetChannelMembers(channel);
                ViewData["Channel"] = channel.Title;
                return View(channelUsers);
            }
            return NotFound();
        }
        public IActionResult RemoveMember(string id, string userName)
        {   
            var channel = _service.GetChannel(id).Result;
            var user = _userManager.GetUserName(User);
            if (user == channel.Creator) 
            {
                if (user != userName) 
                {
                    var channelMember = _service.GetChannelMember(userName, channel).Result;
                    _service.RemoveMember(channelMember);
                }
                return RedirectToAction("Main", "Channel", new { id = channel.Title} );
            }
            return NotFound();
        }

        public IActionResult Create()
        {
            ViewData["Exists"] = false;
            return View();
        }
        public IActionResult Manage(string id)
        {   
            if (id == null) {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            var user = _userManager.GetUserName(User);
            if (user == channel.Creator) 
            {
                return View(channel);
            }
            return NotFound();
        }

        public IActionResult UpdateChannel(string id, string description)
        {   var channel = _service.GetChannel(id).Result;
            var user = _userManager.GetUserName(User);
            if (user == channel.Creator) 
            {
                _service.UpdateChannel(channel, description);
                return RedirectToAction("Main", "Channel", new { id = channel.Title} );
            }
            return NotFound();
        }

        public IActionResult CreateChannel(string title, string description)
        {
            var channel = _service.GetChannel(title).Result;
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
                _service.CreateChannel(channel);
                _service.AddMember(channel, _userManager.GetUserName(User));
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