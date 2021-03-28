using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Login.Models.ChannelList;
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

        public IActionResult Index()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var userChannels = new ChannelList { Channels = _service.GetChannels(user) };
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
            ViewData["public"] = _service.CheckIfPublic(channel);
            ViewData["owner"] = false;
            var user = _userManager.GetUserAsync(User).Result;
            var channelMember = _service.GetChannelMember(user, channel).Result;
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
            var albums = _albumService.GetAlbumModels(channel);
            var members = _service.GetChannelMembers(channel);
            var creator = _userManager.FindByIdAsync(channel.CreatorId).Result;
            var tags = _service.GetChannelTags(channel);
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
        public IActionResult JoinChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            _service.AddMember(channel, user);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }
        public IActionResult RequestToJoin(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }
        public IActionResult LeaveChannel(string id)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            var channelMember = _service.GetChannelMember(user, channel).Result;
            _service.RemoveMember(channelMember);
            return RedirectToAction("Main", "Channel", new { id = channel.Title });
        }

        public IActionResult Delete(string id)
        {   var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId) 
            {
                return View(channel);
            }
            return NotFound();
        }
        public async Task<IActionResult> ConfirmDelete(string id)
        {   var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                await _service.DeleteChannel(channel);
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        public IActionResult RemoveMember(string id, string userName)
        {
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId) 
            {
                if (user.UserName != userName) 
                {
                    var userToRemove = _service.GetByUserName(userName);
                    ViewData["Title"] = channel.Title;
                    return View(userToRemove);
                }
            }
            return NotFound();
        }

        public IActionResult ConfirmRemove(string id, string userName)
        {   
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                if (user.UserName != userName)
                {
                    var userToRemove = _service.GetByUserName(userName);
                    var channelMember = _service.GetChannelMember(userToRemove, channel).Result;
                    _service.RemoveMember(channelMember);
                }
                return RedirectToAction("Main", "Channel", new { id = channel.Title });
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
            if (id == null)
            {
                return NotFound();
            }
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                var tags = _service.GetChannelTags(channel);
                var tagline = "";
                foreach (var tag in tags)
                {
                    tagline+=",";
                    tagline+=tag.Name;
                }
                if (tagline.Length!=0) tagline = tagline.Substring(1);
                ViewData["Tags"] = tagline;
                return View(channel);
            }
            return NotFound();
        }
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
            return NotFound();
        }

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
                var album = _albumService.GetAlbum(channel, Title);
                if (album == null)
                {
                    var albumId = _albumService.CreateNewAlbum(channel, Title, NotVisible, NoPosting);
                    return RedirectToAction("Main", "Album", new { id = albumId });
                }
                else
                {
                    ViewData["channel"] = channel.Title;
                    ViewData["Exists"] = true;
                    return View("CreateAlbum");
                }

            }
            return NotFound();
        }

        public IActionResult UpdateChannel(string id, string description, string tags)
        {   
            var channel = _service.GetChannel(id).Result;
            if (channel == null) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            if (user.Id == channel.CreatorId)
            {
                _service.UpdateChannel(channel, description);
                _service.ChangeTags(channel, tags);
                return RedirectToAction("Main", "Channel", new { id = channel.Title });
            }
            return NotFound();
        }

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
                    Public = !isPrivate,
                    CreationDate = DateTime.Now
                };
                _service.CreateChannel(channel);
                _service.AddMember(channel, user);
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