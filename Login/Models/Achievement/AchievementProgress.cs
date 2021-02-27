using Login.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Models
{
    public class AchievementProgress
    {
        public int Id { get; set; }
        public LoginUser User { get; set; }
        
        public int UsersProgress { get; set; }
        public int MaxProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedTime { get; set; }

        public Achievement Achievement { get; set; }
    }
}
