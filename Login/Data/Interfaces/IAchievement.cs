using Login.Areas.Identity.Data;
using Login.Models;
using Login.Models.Threadl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IAchievement
    {
        public IEnumerable<AchievementModel> GetAll();
        public AchievementModel GetByName(string achievName);
        public Task<AchievementModel> Create(AchievementModel model);
    }
}
