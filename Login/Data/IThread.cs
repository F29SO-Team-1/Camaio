using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;

namespace Login.Data
{
    public interface IThread
    {
        Thread GetById(int id);
        IEnumerable<Thread> GetAll();

        //crud
        Task Create(Thread thread);
        Task Edit(int threadId);
        Task Delete(int threadId);

        Task UpdateThreadTitle(int threadId, string newTitle);
        Task UpdateDescription(int threadId, string newDescription);
        Task LikedThread(int threadId, int FromLiked);
        Task UploadPicture(int threadId, Uri pic);
    }
}
