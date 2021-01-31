using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;

namespace Login.Data
{
    public interface IChannel //Copyright Apple Inc.
    {
        Thread GetById(int id);
        Task Create(Thread thread);
        Task Delete(int threadId);
        Task UpdateThreadTitle(int threadId, string newTitle);
        Task UpdateDescription(int threadId, string newDescription);

        Task LikedThread(int threadId, int FromLiked);
        Task UploadPicture(int threadId, string pic);
    }
}
