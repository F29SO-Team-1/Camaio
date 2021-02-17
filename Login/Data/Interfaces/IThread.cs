using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Login.Areas.Identity.Data;
using Login.Models;

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
        //upload image function
        Task UploadPicture(int threadId, Uri pic);

        //btn work
        bool CheckAreadyLiked(Thread threadId, string userId);
        IEnumerable<Likes> ListOfLikes(int? threadId);
        Task UpdateLikes(int? threadId);

        //like
        Task AddUserToLikeList(int? threadId, string userId);

        //disLike
        Task RemoveUserFromLikeList(int? threadId, string userId);

        //report
        Task Report(int? threadId);

    }
}
