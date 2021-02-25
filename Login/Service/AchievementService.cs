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

    }
}
