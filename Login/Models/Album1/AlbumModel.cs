using Login.Models.Threadl;
using System.Collections.Generic;

namespace Login.Models.Album1
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public Channel Channel { get; set; }
        public IEnumerable<ThreadModel> Threads { get; set; }
    }
}
