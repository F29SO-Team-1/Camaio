using Login.Areas.Identity.Data;
using Login.Models.Threadl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Login.Models.Album1
{
    public class AlbumModel
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public Channel Channel { get; set; }
        public IEnumerable<ThreadModel> Threads { get; set; }
    }
}
