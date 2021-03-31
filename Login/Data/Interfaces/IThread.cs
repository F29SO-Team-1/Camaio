﻿using ExifLib;
using Login.Areas.Identity.Data;
using Login.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IThread
    {
        //returns a Thread by Id
        Thread GetById(int? id);
        //returns all the threads in the database
        IEnumerable<Thread> GetAll();
        //get all the threads that the user liked
        IEnumerable<Thread> GetLikedThreads(string userName);

        //returns the list of the users thread
        IEnumerable<Thread> UserThreads(string userName);
        //returns the list of threads in the album
        IEnumerable<Thread> AlbumThreads(Album album);
        //returns the list of the users thread that are not in any albums
        IEnumerable<Thread> UserThreadsWithoutAlbum(string userName);

        //checks if the thread exists by Id
        bool ThreadExists(int id);
        //makes a thread
        Task<Thread> Create(Thread thread, LoginUser user, int id);
        //edit of the thread
        Task Edit(Thread thread);
        //delete of a thread
        Task Delete(int? threadId);
        //upload image function
        Task UploadPicture(int threadId, Uri pic);

        //btn work
        bool CheckAreadyLiked(Thread threadId, string userId);
        //this is a list of likes on a thread
        IEnumerable<Likes> ListOfLikes(int? threadId);
        Task UpdateLikes(int? threadId);

        //like
        Task AddUserToLikeList(int? threadId, string userId);

        //disLike
        Task RemoveUserFromLikeList(int? threadId, string userId);

        //report

        Task Report(int? threadId, string userName);
        //returns all the reports 
        IEnumerable<Report> ListOfReports(int? threadId);
        //resets all the reports and updates the number count
        Task ResetReports(int? threadId);

        //flags the thread
        Task FlagThread(int? threadId);
        string GetChannelCreator(Thread thread);
        void ChangeTags(Thread thread, string tags);
        IEnumerable<Tag> GetThreadTags(Thread thread);

        //getting cords from the image
        double? GetCoordinate(ExifReader reader, ExifTags type);
        public double ToDoubleCoordinates(double[] coordinates);
        public Task AssignCords(IFormFile file, int threadId);
    }
}
