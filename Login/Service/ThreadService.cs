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

        public Task Edit(int threadId)
        {
            throw new NotImplementedException();
        }

        //returns a list of all the thread
        public IEnumerable<Thread> GetAll()
        {
            return _context.Threads;
        }

        public Thread GetById(int id)
        {
            return GetAll().FirstOrDefault(thread => thread.ID == id);
        }

        public Task LikedThread(int threadId, int FromLiked)
        {
            throw new NotImplementedException();
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
    }
}
