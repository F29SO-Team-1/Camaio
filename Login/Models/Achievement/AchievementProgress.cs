using System;

namespace Login.Models
{
    public class AchievementProgress
    {
        public int Id { get; set; }
        public int UsersProgress { get; set; }
        public int MaxProgress { get; set; }
        public bool Completed { get; set; }
        public DateTime CompletedTime { get; set; }

        public Achievement Achievement { get; set; }
        public string UserId { get; set; }
    }
}
