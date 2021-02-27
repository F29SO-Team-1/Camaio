using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class AchievementService : IAchievement
    {
        private readonly AchievementContext _context;

        public AchievementService(AchievementContext context)
        {
            _context = context;
        }

        public IEnumerable<Achievement> GetAllAchievements()
        {
            return _context.Achievement;
        }

        public async Task<Achievement> MakeAchievement(Achievement model)
        {
            var achieve = new Achievement
            {
                Name = model.Name,
                Description = model.Description,
                Picture = model.Picture,
                ProgressLimit = model.ProgressLimit
            };
            _context.Add(achieve);
            await _context.SaveChangesAsync();
            return achieve;
        }

        public async Task AssignAchievementsToUser(LoginUser user)
        {
            var test = GetAllAchievements().Select(ach => new AchievementProgress
            {
                Achievement = ach,
                Completed = false,
                MaxProgress = ach.ProgressLimit,
                User = user,
                UsersProgress = 0,
            });

            foreach (var achieve in test)
            {
                _context.AchievementProgress.Add(achieve);
            }
            await _context.SaveChangesAsync();
        }

        public IEnumerable<AchievementProgress> GetUsersAchievement(LoginUser user)
        {
            return _context.AchievementProgress.Where(x=>x.User.UserName == user.UserName);
        }


    }
}
