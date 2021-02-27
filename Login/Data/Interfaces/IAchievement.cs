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
        IEnumerable<Achievement> GetAllAchievements();

        //admins only
        Task<Achievement> MakeAchievement(Achievement model);

        Task AssignAchievementsToUser(LoginUser user);

        IEnumerable<AchievementProgress> GetUsersAchievement(LoginUser user);
    }
}
