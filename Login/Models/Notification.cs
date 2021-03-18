using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Login.Models.ApplicationUser;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Login.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Thread Thread { get; set; }
        public Channel Channel { get; set; }
        public Event Event { get; set; }
        public DateTime Date { get; set; }

    }
}