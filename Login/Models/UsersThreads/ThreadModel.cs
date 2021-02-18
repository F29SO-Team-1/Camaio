using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models.Threadl
{
    public class ThreadModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        public string Picture { get; set; }
        public DateTime Created { get; set; }

        public string AuthorId { get; set; }
        public string AuthorUserName { get; set; }

        public int Rating { get; set; }
        public int NoReports { get; set; }
        public bool Flagged { get; set; }

        public IEnumerable<Likes> LikedBy { get; set; }
        

    }
}
