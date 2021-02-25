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
        private readonly LoginContext _context;
        public AchievementService(LoginContext context)
        {
            _context = context;
        }

        //leaving to later
        public async Task<AchievementModel> Create(AchievementModel model)
        {
            var newAchiev = new AchievementModel
            {
                Name = model.Name,
                Description = model.Description
            };

            _context.Achievement.Add(newAchiev);
            await _context.SaveChangesAsync();
            return newAchiev;
        }

        public IEnumerable<AchievementModel> GetAll()
        {
            return _context.Achievement;
        }

        public AchievementModel GetByName(string achievName)
        {
            return GetAll().FirstOrDefault(x => x.Name == achievName);
        }
    }
}
