using Login.Models;
using Login.Models.Threadl;
using Login.Models.Album1;
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
    public class AlbumController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly IChannel _channelService;
        private readonly IAlbum _service;
        private readonly IThread _threadService;

        public AlbumController(UserManager<LoginUser> userManager, IChannel channelService, IAlbum service, IThread threadService)
        {
            _userManager = userManager;
            _channelService = channelService;
            _service = service;
            _threadService = threadService;
        }

        public IActionResult Main(int id)
        {
            if (id == 1) return NotFound();
            var user = _userManager.GetUserAsync(User).Result;
            var album = _service.GetAlbum(id);
            if (album == null) return NotFound();
            ViewData["CanPost"] = false;
            ViewData["CanManage"] = false;
            var channel = _channelService.GetChannel(album.ChannelId).Result;
            if (channel != null) {
                var channelUser = _channelService.GetChannelMember(user, channel).Result;
                if (channelUser == null)
                {
                    if (!album.VisibleToGuests) 
                    {
                        return NotFound();
                    }
                } else 
                {
                    if(user.Id == channel.CreatorId || album.MembersCanPost)
                    {
                        ViewData["CanPost"] = true;
                        if(user.Id == channel.CreatorId)
                        {
                            ViewData["CanManage"] = true;
                        }
                    }
                }
            }
            var threads = BuildThreadList(album);
            var model = new AlbumModel 
            {
                AlbumId = album.Id,
                Title = album.Title,
                Channel = channel,
                Threads = threads
            };
            
            return View(model);
        }

        public IActionResult Delete(int albumId)
        {
            var album = _service.GetAlbum(albumId);
            if (album == null) return NotFound();
            var channel = _service.GetChannel(album);
            if (channel.CreatorId != _userManager.GetUserId(User)) return NotFound();
            return View(album);
        }

        public IActionResult DeleteAlbum(int albumId)
        {
            var album = _service.GetAlbum(albumId);
            if (album == null) return NotFound();
            var channel = _service.GetChannel(album);
            if (channel.CreatorId != _userManager.GetUserId(User)) return NotFound();
            _service.DeleteAlbum(album);
            return RedirectToAction("Main", "Channel", new { id = channel.Title} );
        }

        private IEnumerable<ThreadModel> BuildThreadList(Album album)
        {
            return _threadService.AlbumThreads(album).Select(threads => new ThreadModel
            {
                Title = threads.Title,
                Description = threads.Description,
                Created = threads.CreateDate,
                Picture = threads.Image,
                AuthorUserName = threads.UserName,
                Rating = threads.Votes,
                Id = threads.ID,
                Flagged = threads.Flagged
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}