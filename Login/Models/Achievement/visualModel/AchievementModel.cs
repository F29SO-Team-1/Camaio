using System;

namespace Login.Models
{
    public class AchievementModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int Progress { get; set; }
        public int ProgressLimit { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedTime { get; set; }

    }
}
