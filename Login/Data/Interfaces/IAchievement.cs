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

        //checks if the user already has the following achievement
        Task GiveFirstLoginAchievement(LoginUser user);

        //checks if the user already has the following achievement; true for yes, false for no
        bool CheckProgression(LoginUser user, int AchievementId);
    }
}
