using System;

namespace Login.Models
{
    public class PostModel
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public Object Picture { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Location { get; set; }

        public int Votes { get; set; }

        public string User { get; set; }

        //constructor
        public PostModel()
        {

        }



    }
}
