using Login.Areas.Identity.Data;
using System;

namespace Login.Models
{
    public class AchievementProgress
    {
        public string Id { get; set; }
        public LoginUser UserName { get; set; }
        
        public int Progress { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedTime { get; set; }

        public Achievement Achievement { get; set; }
    }
}
