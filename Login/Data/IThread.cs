using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Areas.Identity.Data;
using Login.Models;
using Login.Models.Threadl;

namespace Login.Data
{
    public interface IThread
    {
        //returns a Thread by Id
        Thread GetById(int? id);
        //returns all the threads in the database
        IEnumerable<Thread> GetAll();

        //returns the list of the users thread
        IEnumerable<Thread> UserThreads(string userName);

        //checks if the thread exists by Id
        bool ThreadExists(int id);
        //makes a thread
        Task<Thread> Create(Thread thread, LoginUser user);
        //edit of the thread
        Task Edit(Thread thread);
        //delete of a thread
        Task Delete(int? threadId);

        //like functions
        Task IncrementRating(int? threadId);

        //upload image function
        Task UploadPicture(int threadId, Uri pic);

        Task AddUserToLikeList(int? threadId, LoginUser userId);
    }
}
