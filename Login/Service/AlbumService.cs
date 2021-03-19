using Login.Data;
using Login.Models;
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
                .Select(table => table)
                .FirstOrDefault();
            return album;
        }
        public Album GetAlbum(int id)
        {
            var album = _context.Albums
                .Where(table => table.Id == id)
                .Select(table => table)
                .FirstOrDefault();
            return album;
        }
    }
}
