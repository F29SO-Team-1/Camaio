using System;
using System.Collections.Generic;
using Login.Areas.Identity.Data;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Login.Models.ApplicationUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Login.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public LoginUser UserName { get; set; }
        public Channel Channel { get; set; }
        public bool VisibleToGuests { get; set; }
        public bool MembersCanPost { get; set; }
        public IEnumerable<Thread> Threads { get; set; }

    }
}