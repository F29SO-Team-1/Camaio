using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
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
        public ThreadService(ThreadContext context)
        {
            _context = context;
        }

        public async Task<Thread> Create(Thread model, LoginUser user)
        {
            var thread = new Thread
            {
                Title = model.Title,
                CreateDate = DateTime.Now,
                Description = model.Description,
                ID = model.ID,
                UserID = user.Id,
                Votes = model.Votes,
                UserName = user.UserName
            };

            _context.Add(thread);
            await _context.SaveChangesAsync();
            return thread;
        }


        public async Task Delete(int? threadId)
        {
            var threadPrimaryKey = await _context.Threads.FindAsync(threadId);
            _context.Threads.Remove(threadPrimaryKey);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Thread thread)
        {
            _context.Update(thread);
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

        public async Task RemoveUserFromLikeList(int? threadId, string userId)
        {
            var thread = GetById(threadId);
            //remove from list
            var liked = await _context.Likes
                .Where(x => x.Thread.ID == threadId)
                .Where(x=> x.UserId == userId)
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
                if (r.UserName == userName) continue;
            }

            _context.Reports.Add(reported);
            await _context.SaveChangesAsync();

        }
        //makes a list of reports for a thread
        public IEnumerable<Report> ListOfReports(int? threadId)
        {
            return _context.Reports.Where(report => report.Thread.ID == threadId);
        }
    }
}