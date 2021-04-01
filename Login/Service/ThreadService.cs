using ExifLib;
using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class ThreadService : IThread
    {

        private readonly ThreadContext _context;
        private readonly TagContext _tagContext;
        private readonly UserManager<LoginUser> _userManager;
        public ThreadService(ThreadContext context, TagContext tagContext, UserManager<LoginUser> userManager)
        {
            _context = context;
            _tagContext = tagContext;
            _userManager = userManager;
        }

        public async Task<Thread> Create(Thread model, LoginUser user, int albumId)
        {
            var thread = new Thread
            {
                Title = model.Title,
                CreateDate = DateTime.Now,
                Description = model.Description,
                UserID = user.Id,
                AlbumId = albumId,
                Votes = model.Votes,
                UserName = user.UserName,
                Flagged = false,
                NoReports = 0
            };

            _context.Add(thread);
            await _context.SaveChangesAsync();
            return thread;
        }


        public async Task Delete(int? threadId)
        {
            var threadPrimaryKey = await _context.Threads.FindAsync(threadId);
            //delete all FKeys; report, like
            await DeleteForeignKeys(threadId);
            _context.Threads.Remove(threadPrimaryKey);
            await _context.SaveChangesAsync();
        }
        private async Task DeleteForeignKeys(int? threadId)
        {
            //get all the records that are accosiated with the threadId
            _context.Likes.RemoveRange(_context.Likes.Where(x => x.Thread.ID == threadId));
            _context.Reports.RemoveRange(_context.Reports.Where(y => y.Thread.ID == threadId));
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Thread thread)
        {
            _context.Update(thread);
            thread.Flagged = false;
            await ResetReports(thread.ID);
            await _context.SaveChangesAsync();
        }

        //returns a list of all the thread
        public IEnumerable<Thread> GetAll()
        {
            return _context.Threads;
        }

        public Thread GetById(int? id)
        {
            return GetAll().FirstOrDefault(thread => thread.ID == id);
        }

        //list of all users post
        public IEnumerable<Thread> UserThreads(string userName)
        {
            return GetAll().Where(thread => thread.UserName == userName);
        }
        //list of all users post that are not a part of an album
        public IEnumerable<Thread> UserThreadsWithoutAlbum(string userName)
        {
            return GetAll()
                .Where(thread => thread.UserName == userName)
                .Where(thread => thread.AlbumId == 1);
        }
        //list of all album threads
        public IEnumerable<Thread> AlbumThreads(Album album)
        {
            return GetAll().Where(thread => thread.AlbumId == album.Id);
        }

        public async Task UploadPicture(int threadId, Uri pic)
        {
            var thread = GetById(threadId);
            thread.Image = pic.AbsoluteUri;
            _context.Update(thread);
            await _context.SaveChangesAsync();
        }

        public bool ThreadExists(int id)
        {
            return _context.Threads.Any(x => x.ID == id);
        }

        public async Task AddUserToLikeList(int? threadId, string userId)
        {
            var thread = GetById(threadId);
            //add user to a list
            var liked = new Likes
            {
                Thread = thread,
                UserId = userId
            };
            _context.Likes.Add(liked);
            await _context.SaveChangesAsync();
            await UpdateLikes(threadId);
        }
        public bool CheckAreadyLiked(Thread threadId, string userId)
        {
            var record = _context.Likes
                .Where(like => like.UserId == userId)
                .Where(l => l.Thread == threadId);

            if (record.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public IEnumerable<Likes> ListOfLikes(int? threadId)
        {
            return _context.Likes.Where(l => l.Thread.ID == threadId);
        }

        public IEnumerable<Thread> GetLikedThreads(string userId)
        {
            //get all the liked threads; and select the threads were the userID is mentioned
            return _context.Likes.Select(x => x.Thread)
                .Where(y => y.Votes >= 1)
                .Where(z => z.LikedBy.Any(v => v.UserId == userId))
                .Distinct();
        }

        public async Task RemoveUserFromLikeList(int? threadId, string userId)
        {
            var thread = GetById(threadId);
            //remove from list
            var liked = await _context.Likes
                .Where(x => x.Thread.ID == threadId)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            _context.Likes.Remove(liked);
            await _context.SaveChangesAsync();
            await UpdateLikes(threadId);
        }

        public async Task UpdateLikes(int? threadId)
        {
            Thread t = GetById(threadId);
            var q = ListOfLikes(threadId).Count();
            t.Votes = q;
            await _context.SaveChangesAsync();
        }

        public async Task Report(int? threadId, string userName)
        {
            //get the thread
            var thread = GetById(threadId);
            //add user to a list
            var reported = new Report
            {
                Thread = thread,
                UserName = userName
            };

            //check if the user already report the same thread
            foreach (Report r in ListOfReports(threadId))
            {
                //if found already will just exit and do nothing
                if (r.UserName == userName) return;
            }

            _context.Reports.Add(reported);
            await _context.SaveChangesAsync();
            await UpdateReports(threadId);
        }
        private async Task UpdateReports(int? threadId)
        {
            Thread t = GetById(threadId);
            var q = ListOfReports(threadId).Count();
            t.NoReports = q;
            await _context.SaveChangesAsync();
        }
        public string GetChannelCreator(Thread thread)
        {
            var albumId = _context.Threads
                .Where(t => t.ID == thread.ID)
                .Select(t => t.AlbumId)
                .FirstOrDefault();
            if (albumId != 1)
            {
                return _context.Threads
                    .Where(t => t.ID == thread.ID)
                    .Select(t => _userManager.FindByIdAsync(t.Album.Channel.CreatorId).Result.UserName)
                    .FirstOrDefault();
            }
            return null;
        }

        //makes a list of reports for a thread
        public IEnumerable<Report> ListOfReports(int? threadId)
        {
            return _context.Reports.Where(report => report.Thread.ID == threadId);
        }

        //deletes all the reports for the current thread/post
        public async Task ResetReports(int? threadId)
        {
            //delete all the reports with the spesific thread Id 
            _context.Reports.RemoveRange(_context.Reports.Where(x => x.Thread.ID == threadId));
            await _context.SaveChangesAsync();
            await UpdateReports(threadId);
        }

        public async Task FlagThread(int? threadId)
        {
            Thread t = GetById(threadId);
            t.Flagged = true;
            await _context.SaveChangesAsync();
        }
        public IEnumerable<Tag> GetThreadTags(Thread thread)
        {
            return _tagContext.Tags
                .Where(tag => tag.ThreadId == thread.ID)
                .Distinct()
                .ToList();
        }
        public void ChangeTags(Thread thread, string tags)
        {
            _context.RemoveRange(_tagContext.Tags.Where(tag => tag.ThreadId == thread.ID && tag.ThreadId != 0).Distinct().ToList());
            _context.AddRange(GetTagList(thread, tags));
            _context.SaveChanges();
        }
        private List<Tag> GetTagList(Thread thread, string tags)
        {
            List<Tag> tagList = new List<Tag>();
            string tag = "";
            if (tags == null) return tagList;
            while (tags.Length != 0)
            {
                if (tags.ElementAt(0).Equals((char)32) || tags.ElementAt(0).Equals((char)44))
                {
                    if (tag.Length > 1)
                    {
                        tagList.Add(new Tag
                        {
                            Name = tag,
                            Thread = thread
                        });
                    }
                    tag = "";
                    tags = tags.Substring(1);
                }
                else
                {
                    tag += (tags.ElementAt(0));
                    if (tags.Length == 1 && tag.Length > 1)
                    {
                        tagList.Add(new Tag
                        {
                            Name = tag,
                            Thread = thread
                        });
                    }
                    tags = tags.Substring(1);
                }
            }
            return tagList;
        }

        public async Task AssignCords(IFormFile file, int threadId)
        {
            try
            {
                using var reader = new ExifReader(file.OpenReadStream());
                var lat = GetCoordinate(reader, ExifTags.GPSLatitude);
                var lng = GetCoordinate(reader, ExifTags.GPSLongitude);

                reader.GetTagValue(ExifTags.GPSLatitude, out double[] Latitude);
                reader.GetTagValue(ExifTags.GPSLongitude, out double[] Longitude);

                Thread thread = GetById(threadId);
                thread.Lat = lat;
                thread.Lng = lng;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return;
            }

        }

        public double? GetCoordinate(ExifReader reader, ExifTags type)
        {
            if (reader.GetTagValue(type, out double[] coordinates))
            {
                return ToDoubleCoordinates(coordinates);
            }

            return null;
        }

        public double ToDoubleCoordinates(double[] coordinates)
        {
            return coordinates[0] + coordinates[1] / 60f + coordinates[2] / 3600f;
        }
    }
}