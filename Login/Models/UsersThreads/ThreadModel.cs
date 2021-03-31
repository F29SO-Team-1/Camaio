using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        //location
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public IEnumerable<Likes> LikedBy { get; set; }

        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }

    }
}
