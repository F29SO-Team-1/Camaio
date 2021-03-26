using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;
using Login.Models.Album1;
using Login.Areas.Identity.Data;

namespace Login.Data
{
    public interface IAlbum //Copyright Apple Inc.
    {
        int CreateNewAlbum(Channel channel, string Title, bool NotVisible, bool NoPosting);
        Album GetAlbum(Channel channel, string Title);
        Album GetAlbum(int id);
        string GetAlbumImage(Album album);
        Channel GetChannel(Album album);
        void DeleteAlbum(Album album);
        IEnumerable<Album> GetAll();
        IEnumerable<AlbumModel> GetAlbumModels(Channel channel);
    }
}
