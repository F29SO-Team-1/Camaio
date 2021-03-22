using System;
using System.Collections.Generic;
using Login.Models.Album1;
using Login.Areas.Identity.Data;


namespace Login.Models
{
    public class ChannelModel
    {
        public int Id { get; set; }
        public LoginUser Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<LoginUser> ChannelMembers { get; set; }
        public IEnumerable<AlbumModel> Albums { get; set; }

    }
}