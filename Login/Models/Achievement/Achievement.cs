﻿namespace Login.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public int ProgressLimit { get; set; }

        //public AchievementProgress AchievementProgress { get; set; }
    }
}
