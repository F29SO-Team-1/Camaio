using Login.Models;
using Login.Models.Search;
using Login.Models.Threadl;
using Login.Models.ApplicationUser;
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
    public class SearchController : Controller
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly IChannel _channelService;
        private readonly IThread _threadService;
        private readonly IAlbum _albumService;
        private readonly IApplicationUsers _userService;

        public SearchController(UserManager<LoginUser> userManager, IChannel channelService, IThread threadService, IApplicationUsers userService, IAlbum albumService)
        {
            _userManager = userManager;
            _channelService = channelService;
            _threadService = threadService;
            _userService = userService;
            _albumService = albumService;
        }

        public IActionResult Index(string searchInput, string searchArea, string searchOptions, string sortingOptions)
        {
            var searchResult = new SearchModel {
                ThreadsIncluded = false,
                ChannelsIncluded = false,
                UsersIncluded = false
            };
            var keywords = GetSearchKeywords(searchInput);
            if (searchArea.Equals("All")||searchArea.Equals("Threads"))
            {
                searchResult.ThreadsIncluded = true;
                searchResult.Threads = GetThreads(keywords, searchOptions, sortingOptions);
            }
            if (searchArea.Equals("All")||searchArea.Equals("Channels"))
            {
                searchResult.ChannelsIncluded = true;
                searchResult.Channels = GetChannels(keywords, searchOptions, sortingOptions);
            }
            if (searchArea.Equals("All")||searchArea.Equals("Users"))
            {
                searchResult.UsersIncluded = true;
                searchResult.Users = GetUsers(keywords, searchOptions, sortingOptions);
            }
            return View(searchResult);
        }
        private List<string> GetSearchKeywords(string searchInput)
        {
            List<string> keywords = new List<string>();
            string keyword = "";
            if (searchInput==null) return keywords;
            while (searchInput.Length!=0)
            {
                if (searchInput.ElementAt(0).Equals((char)32) || searchInput.ElementAt(0).Equals((char)44))
                {
                    if(keyword.Length>1)
                    {
                        keywords.Add(keyword);
                    }
                    keyword = "";
                    searchInput = searchInput.Substring(1);
                } 
                else
                {
                    keyword+=(searchInput.ElementAt(0));
                    if (searchInput.Length==1 && keyword.Length>1)
                    {
                        keywords.Add(keyword);
                    }
                    searchInput = searchInput.Substring(1);
                }
            }
            return keywords;
        }
        private bool StringContains(string originalString, string keyword)
        {
            return originalString.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private IEnumerable<ThreadModel> GetThreads(List<string> keywords, string searchOptions, string sortingOptions)
        {
            var threads = _threadService.GetAll().Where(thread => thread.AlbumId==1);
            if (searchOptions.Equals("Keywords"))
            {
                foreach (var keyword in keywords)
                {
                    var oneStepLookup = threads.Where(thread => StringContains(thread.Title, keyword) || StringContains(thread.Description, keyword));
                    if (oneStepLookup.FirstOrDefault()!=null) threads = oneStepLookup;
                }
            }
            else if (searchOptions.Equals("Tags"))
            {
                return null;
            }
            if (sortingOptions.Equals("Votes"))
            {
                threads = threads.OrderByDescending(thread => thread.Votes);
            }
            else 
            {
                threads = threads.OrderByDescending(thread => thread.CreateDate);
            }
            return threads.Select(thread => new ThreadModel {
                Title = thread.Title,
                Rating = thread.Votes,
                Description = thread.Description,
                Picture = thread.Image,
                Created = thread.CreateDate
            });
        }
        private IEnumerable<ChannelModel> GetChannels(IEnumerable<string> keywords, string searchOptions, string sortingOptions)
        {
            var channels = _channelService.GetAll();
            if (searchOptions.Equals("Keywords"))
            {
                foreach (var keyword in keywords)
                {
                    var oneStepLookup = channels.Where(channel => StringContains(channel.Title, keyword) || StringContains(channel.Description, keyword));
                    if (oneStepLookup.FirstOrDefault()!=null) channels = oneStepLookup;
                }
            }
            else if (searchOptions.Equals("Tags"))
            {
                return null;
            }
            if (sortingOptions.Equals("Votes"))
            {
                channels.OrderByDescending(channel => GetChannelRating(channel));
            }
            else 
            {
                channels = channels.OrderByDescending(channel => channel.CreationDate);
            }
            return channels.Select(channel => new ChannelModel {
                Title = channel.Title,
                ChannelRating = 0,
                Description = channel.Description,
                CreationDate = channel.CreationDate
            });
        }
        private int GetChannelRating(Channel channel)
        {
            var rating = _threadService.GetAll()
                    .Where(thread => thread.Album.Channel == channel)
                    .Where(thread => thread.AlbumId != 1)
                    .Sum(thread => thread.Votes);
            return rating;
        }
        private IEnumerable<ProfileModel> GetUsers(IEnumerable<string> keywords, string searchOptions, string sortingOptions)
        {
            var users = _userService.GetAll();
            if (searchOptions.Equals("Keywords"))
            {
                foreach (var keyword in keywords)
                {
                    var oneStepLookup = users.Where(user => StringContains(user.UserName, keyword));
                    if (oneStepLookup.FirstOrDefault()!=null) users = oneStepLookup;
                }
            }
            else if (searchOptions.Equals("Tags"))
            {
                return null;
            }
            if (sortingOptions.Equals("Votes"))
            {
                users = users.OrderByDescending(users => users.Ratting);
            }
            else 
            {
                users = users.OrderByDescending(user => user.MemberSince);
            }
            return users.Select(user => new ProfileModel {
                Username = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl,
                UserRating = user.Ratting,
                MemmberSince = user.MemberSince

            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}