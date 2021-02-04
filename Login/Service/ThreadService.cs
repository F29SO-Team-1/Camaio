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

        public Task Create(Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int threadId)
        {
            throw new NotImplementedException();
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
            return GetAll().Where(thread => thread.UserID == userName);
        }

        //adds +1 to when you press the button
        public async Task IncrementRating(int? threadId)
        {
            var thread = GetById(threadId);
            thread.Votes = thread.Votes + 1;
            _context.Update(thread);
            await _context.SaveChangesAsync();
        }

        public Task UpdateDescription(int threadId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateThreadTitle(int threadId, string newTitle)
        {
            throw new NotImplementedException();
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
    }
}