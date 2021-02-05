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
        Thread GetById(int? id);
        IEnumerable<Thread> GetAll();

        //will have to return a list of threads 
        IEnumerable<Thread> UserThreads(string userName);

        bool ThreadExists(int id);

        Task Create(Thread thread);
        Task Edit(Thread thread);
        Task Delete(int threadId);

        Task UpdateThreadTitle(int threadId, string newTitle);
        Task UpdateDescription(int threadId, string newDescription);
        Task IncrementRating(int? threadId);
        Task UploadPicture(int threadId, Uri pic);


    }
}
