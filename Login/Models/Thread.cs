using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models
{
    public class Thread
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int PostedIn { get; set; }
        public string Location { get; set; }
        public int Votes { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
