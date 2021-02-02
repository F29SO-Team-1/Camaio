using Login.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models.Threadl
{
    public class ThreadModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public DateTime Created { get; set; }

        public string AuthorId { get; set; }

        public IFormFile Image { get; set; }

    }
}
