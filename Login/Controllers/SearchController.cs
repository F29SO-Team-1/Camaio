using Login.Models;
using Login.Models.Search;
using Login.Models.Threadl;
using Login.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        [ValidateAntiForgeryToken]
        public IActionResult Index(string searchInput, string searchArea, string searchOptions, string sortingOptions)
        {
            var searchResult = new SearchModel {
                ThreadsIncluded = false,
                ChannelsIncluded = false,
                UsersIncluded = false
            };
            var keywords = GetSearchKeywords(searchInput);  //Returns a list of search keywords
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
            if (searchInput==null) return keywords;  //If input is null, then return an empty list
            while (searchInput.Length!=0)  //Loops through the entire input
            {
                if (searchInput.ElementAt(0).Equals((char)32) || searchInput.ElementAt(0).Equals((char)44)) //ASCII for whitespace and coma
                {
                    if(keyword.Length>1)
                    {
                        keywords.Add(keyword); //Adds a keyword to the list if it is 2 or more characters long
                    }
                    keyword = ""; //reset the current keyword
                    searchInput = searchInput.Substring(1); //remove the first input character
                } 
                else
                {
                    keyword+=(searchInput.ElementAt(0)); //adds a character to the current keyword if it is not whitespace or coma
                    if (searchInput.Length==1 && keyword.Length>1)
                    {
                        keywords.Add(keyword); //Adds a keyword to the list if it is 2 or more characters long
                    }
                    searchInput = searchInput.Substring(1); //remove the first input character
                }
            }
            return keywords;
        }
        //Returns true if originalString either contains or equal to the keyword
        private bool StringContains(string originalString, string keyword)
        {
            if (originalString==null) return false;
            return originalString.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private IEnumerable<ThreadModel> GetThreads(List<string> keywords, string searchOptions, string sortingOptions)
        {
            var threads = _threadService.GetAll().Where(thread => thread.AlbumId==1);
            // var tags = _channelService.GetAllTags();
            // if (searchOptions.Equals("Keywords"))
            // {
                foreach (var keyword in keywords) //Performs one step lookup further into the search. If the next keyword lookup produces an empty result, then skip it
                {
                    var oneStepLookup = threads.Where(thread => StringContains(thread.Title, keyword) || StringContains(thread.Description, keyword));
                    if (oneStepLookup.FirstOrDefault()!=null) threads = oneStepLookup;
                }
            // }
            // else if (searchOptions.Equals("Tags"))
            // {
            //     foreach (var keyword in keywords)
            //     {
            //         var oneStepLookup = tags
            //             .Where(tag => StringContains(tag.Name, keyword))
            //             .Where(tag => tag.ThreadId!=0);
            //         if (oneStepLookup.FirstOrDefault()!=null)
            //         {
            //             tags = oneStepLookup;
            //             threads = tags.Select(tag => tag.Thread).Distinct(); 
            //         }
            //     }
            // }
            if (sortingOptions.Equals("Votes"))
            {
                threads = threads.OrderByDescending(thread => thread.Votes);
            }
            else 
            {
                threads = threads.OrderByDescending(thread => thread.CreateDate);
            }
            return threads.Select(thread => new ThreadModel {
                Id = thread.ID,
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
            // if (searchOptions.Equals("Keywords"))
            // {
                foreach (var keyword in keywords) //Performs one step lookup further into the search. If the next keyword lookup produces an empty result, then skip it
                {
                    var oneStepLookup = channels.Where(channel => StringContains(channel.Title, keyword) || StringContains(channel.Description, keyword));
                    if (oneStepLookup.FirstOrDefault()!=null) channels = oneStepLookup;
                }
            // }
            // else if (searchOptions.Equals("Tags"))
            // {
            //     var tags = _channelService.GetAllTags();
            //     foreach (var keyword in keywords)
            //     {
            //         var oneStepLookup = tags
            //             .Where(tag => StringContains(tag.Name, keyword))
            //             .Where(tag => tag.ChannelId!=0);
            //         if (oneStepLookup.FirstOrDefault()!=null) tags = oneStepLookup;
            //     }
            //     if(tags.Select(tag => tag.Channel).FirstOrDefault()!=null) channels = tags.Select(tag => tag.Channel).Distinct();
            // }
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
            foreach (var keyword in keywords) //Performs one step lookup further into the search. If the next keyword lookup produces an empty result, then skip it
            {
                var oneStepLookup = users.Where(user => StringContains(user.UserName, keyword));
                if (oneStepLookup.FirstOrDefault()!=null) users = oneStepLookup;
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