using Login.Models;
using Login.Models.Album1;
using System.Collections.Generic;

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
        IEnumerable<AlbumModel> GetAlbumModels(Channel channel);
    }
}
