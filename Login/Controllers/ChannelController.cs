using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.ChannelList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Login.Controllers
{
    public class ChannelController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly IChannel _service;
        private readonly IAlbum _albumService;

        public ChannelController(UserManager<LoginUser> userManager, IChannel service, IAlbum albumService)
        {
            _userManager = userManager;
            _service = service;
            _albumService = albumService;
        }
        //User channel list
        [Authorize]
        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var userChannels = new ChannelList { Channels = _service.GetChannels(user) };
            return View(userChannels);
        }
        //Channel main page
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
            ViewData["public"] = _service.CheckIfPublic(channel);
            ViewData["owner"] = false;
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null) 
            {
                var channelMember = _service.GetChannelMember(user, channel).Result; //Check if the user is a channel member
                if (channelMember == null)
                {
                    ViewData["member"] = false;
                }
                else
                {
                    ViewData["member"] = true;
                }
                if (channel.CreatorId == user.Id)
                {
                    ViewData["owner"] = true;
                }
            }
            var albums = _albumService.GetAlbumModels(channel); //List of all channel albums
            var members = _service.GetChannelMembers(channel); //List of all channel members
            var creator = _userManager.FindByIdAsync(channel.CreatorId).Result; //Channel creator
            var tags = _service.GetChannelTags(channel); //List of all channel tags
            var channelModel = new ChannelModel
            {
                Id = channel.Id,
                Title = channel.Title,
                Description = channel.Description,
                Creator = creator,
                CreationDate = channel.CreationDate,
                Albums = albums,
                ChannelMembers = members,
                Tags = tags
            };
            return View(channelModel);
        }
        [Authorize]
        public IActionResult JoinChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            _service.AddMember(channel, user);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }
        [Authorize]
        public IActionResult RequestToJoin(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }
        [Authorize]
        public IActionResult LeaveChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            var channelMember = _service.GetChannelMember(user, channel).Result;
            _service.RemoveMember(channelMember);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }
        //Delete the channel
        [Authorize]
        public IActionResult Delete(string id)
        {   var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod")) 
            {
                return View(channel);
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(string id)
        {   var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod"))
            {
                await _service.DeleteChannel(channel);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        //Kick a channel member
        [Authorize]
        public IActionResult RemoveMember(string id, string userName)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod")) 
            {
                if (channel.CreatorId != _service.GetByUserName(userName).Id)
                {
                    var userToRemove = _service.GetByUserName(userName);
                    ViewData["Title"] = channel.Title;
                    return View(userToRemove);
                }
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        public IActionResult ConfirmRemove(string id, string userName)
        {   
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod"))
            {
                if (user.UserName != userName)
                {
                    var userToRemove = _service.GetByUserName(userName);
                    var channelMember = _service.GetChannelMember(userToRemove, channel).Result; //Get the joint table between users and channels
                    _service.RemoveMember(channelMember);
                }
                return RedirectToAction("Main", "Channel", new { id = channel.Title });
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        //Create a channel
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Exists"] = false;
            return View();
        }
        [Authorize]
        public IActionResult Manage(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod"))
            {
                var tags = _service.GetChannelTags(channel);
                var tagline = "";
                foreach (var tag in tags) //creates a line of tags separated by comas
                {
                    tagline+=",";
                    tagline+=tag.Name;
                }
                if (tagline.Length!=0) tagline = tagline.Substring(1);
                ViewData["Tags"] = tagline; //Used to display it
                return View(channel);
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        public IActionResult CreateAlbum(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                ViewData["Exists"] = false;
                ViewData["channel"] = channel.Title;
                return View();
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult NewAlbum(string id, string Title, bool NotVisible, bool NoPosting)
        {
            if (id == null)
            {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                var album = _albumService.GetAlbum(channel, Title); //Check if an album with this title already exists in this channel
                if (album == null)
                {
                    var albumId = _albumService.CreateNewAlbum(channel, Title, NotVisible, NoPosting);
                    return RedirectToAction("Main", "Album", new { id = albumId });
                }
                else
                {
                    ViewData["channel"] = channel.Title;
                    ViewData["Exists"] = true;  //Displays a message that an album with this title already exists
                    return View("CreateAlbum");
                }

            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateChannel(string id, string description, string tags)
        {   
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId || User.IsInRole("Admin") || User.IsInRole("Mod"))
            {
                _service.UpdateChannel(channel, description);
                _service.ChangeTags(channel, tags);
                return RedirectToAction("Main", "Channel", new { id = channel.Title });
            }
            return RedirectToAction("Index", "Thread", new { id = 30 });
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CreateChannel(string title, string description, bool isPrivate, string tags)
        {
            var channel = _service.GetChannel(title).Result;
            var user = _userManager.GetUserAsync(User).Result;
            if (channel == null)
            {
                channel = new Channel()
                {
                    Creator = user,
                    Title = title,
                    Description = description,
                    Public = true,
                    CreationDate = DateTime.Now
                };
                _service.CreateChannel(channel);
                _service.AddMember(channel, user); //Adds the creator as a channel member
                _service.ChangeTags(channel, tags);
                return RedirectToAction("Main", "Channel", new { id = channel.Title} );

            }
            else
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