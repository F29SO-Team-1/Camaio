using Login.Data;
using Login.Models;
using Login.Models.Album1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Areas.Identity.Data;

namespace Login.Service
{
    public class AlbumService : IAlbum
    {
        private readonly ChannelContext _context;
        public AlbumService(ChannelContext context)
        {
            _context = context;
        }

        public int CreateNewAlbum(Channel channel, string title, bool NotVisible, bool NoPosting)
        {

            var album = new Album()
                {
                    Title = title,
                    Channel = channel,
                    VisibleToGuests = !NotVisible,
                    MembersCanPost = !NoPosting
                };
            _context.Add(album);
            _context.SaveChanges();
            return GetAlbum(channel, title).Id;

        }
        public Album GetAlbum(Channel channel, string title)
        {
            var album = _context.Albums
                .Where(table => table.Channel == channel)
                .Where(table => table.Title == title)
                .FirstOrDefault();
            return album;
        }
        public Album GetAlbum(int id)
        {
            var album = _context.Albums
                .Where(table => table.Id == id)
                .FirstOrDefault();
            return album;
        }
        public Channel GetChannel(Album album)
        {
            return _context.Albums
                .Where(a => a.Id == album.Id)
                .Select(a => a.Channel)
                .FirstOrDefault();
        }
        public void DeleteAlbum(Album album)
        {
            _context.RemoveRange(album);
            _context.SaveChanges();
        }
        public IEnumerable<AlbumModel> GetAlbumModels(Channel channel)
        {
            return _context.Albums
                .Where(album => album.ChannelId == channel.Id)
                .Where(album => album.Id != 1)
                .Select(album => new AlbumModel 
                {
                    AlbumId = album.Id,
                    Title = album.Title,
                    Channel = album.Channel
                })
                .ToList();
        }

        public string GetAlbumImage(Album album)
        {
            var image = _context.Albums
                .Where(a => a == album)
                .Select(a => a.Threads.FirstOrDefault().Image)
                .FirstOrDefault();
            if(image == null)
            {
                return "https://camaiologinstorage.blob.core.windows.net/thread-storage/Thumbs_Up_Skin-Color.pngtest2@gmail.com637516981283394465";
            } else 
            {
                return image;
            }
        }
    }
}
