using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace Login.Models
{
    public class Post
    {
        [Key]
        public int Post_id { get; set; }
        public string Post_title { get; set; }
        public string Post_description { get; set; }
        public string Post_picture { get; set; }
        public string Post_location { get; set; }
        public int Post_votes { get; set; }
        public string Post_user { get; set; }
    }
}