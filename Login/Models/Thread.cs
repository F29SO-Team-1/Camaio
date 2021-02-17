using System;
using System.Collections.Generic;

namespace Login.Models
{
    public class Thread
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int Votes { get; set; }

        public IEnumerable<Likes> LikedBy { get; set; }

        public IEnumerable<Report> Reports { get; set; }
    }
}
