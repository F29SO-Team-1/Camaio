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

        public Achievement GetById(int id)
        {
            return GetAllAchievements().FirstOrDefault(x => x.Id == id);
        }
        public async Task UploadPicture(int achId, Uri pic)
        {
            var ach = GetById(achId);
            ach.Picture = pic.AbsoluteUri;
            _context.Update(ach);
            await _context.SaveChangesAsync();
        }

        public async Task<Achievement> MakeAchievement(Achievement model)
        {
            var achieve = new Achievement
            {
                Id = model.Id,
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
            var uAch = GetUsersAchievement(user).ToList();
            var uuAch = new List<Achievement>();
            var aAch = GetAllAchievements().ToList();
            var toDB = new List<AchievementProgress>();

            if (uAch.Count() == aAch.Count())
            {
                return;
            }

            //when there are 0 and you need to add one
            if (uAch.Count() == 0)
            {
                if (aAch.Count() == 1)
                {
                    foreach (var toAdd in aAch)
                    {
                        var newAchPro = new AchievementProgress
                        {
                            Achievement = toAdd,
                            Completed = false,
                            MaxProgress = toAdd.ProgressLimit,
                            UserId = user.Id,
                            UsersProgress = 0
                        };
                        toDB.Add(newAchPro);
                    }
                    var first = toDB.FirstOrDefault();
                    _context.AchievementProgress.Add(first);
                    await _context.SaveChangesAsync();
                    return;
                }
                else
                {
                    foreach (var toAdd in aAch)
                    {
                        var newAchPro = new AchievementProgress
                        {
                            Achievement = toAdd,
                            Completed = false,
                            MaxProgress = toAdd.ProgressLimit,
                            UserId = user.Id,
                            UsersProgress = 0
                        };
                        toDB.Add(newAchPro);
                    }
                    await _context.AchievementProgress.AddRangeAsync(toDB);
                    await _context.SaveChangesAsync();
                    return;
                }
            }

            /*
             * gets the difference in lists; the total amount of lists and the users achievement
             * converts progress list to a achv list
             */
            foreach (var usersAch in uAch)
            {
                uuAch.Add(usersAch.Achievement);
            }
            //gets the two lists and gets the difference between the two
            var change = aAch.Except(uuAch).ToList();


            if (change.Count() > 1)
            {
                foreach (var toAdd in change)
                {
                    var newAchPro = new AchievementProgress
                    {
                        Achievement = toAdd,
                        Completed = false,
                        MaxProgress = toAdd.ProgressLimit,
                        UserId = user.Id,
                        UsersProgress = 0
                    };
                    toDB.Add(newAchPro);
                }
                await _context.AchievementProgress.AddRangeAsync(toDB);
                await _context.SaveChangesAsync();
                return;
            }
            else
            {
                foreach (var toAdd in change)
                {
                    var newAchPro = new AchievementProgress
                    {
                        Achievement = toAdd,
                        Completed = false,
                        MaxProgress = toAdd.ProgressLimit,
                        UserId = user.Id,
                        UsersProgress = 0
                    };
                    toDB.Add(newAchPro);
                }
                _context.AchievementProgress.Add(toDB.FirstOrDefault());
                await _context.SaveChangesAsync();
                return;
            }
        }

        public IEnumerable<AchievementProgress> GetUsersAchievement(LoginUser user)
        {
            return _context.AchievementProgress.Where(x => x.UserId == user.Id);
        }

        public AchievementProgress GetUsersAchievementProgress(LoginUser user, int achievementId)
        {
            return _context.AchievementProgress
                .Where(x => x.UserId == user.Id)
                .FirstOrDefault(y => y.Achievement.Id == achievementId);
        }

        public bool CheckProgression(LoginUser user, int achievementId)
        {
            if (GetUsersAchievement(user).Count() == 0)
            {
                return false;
            }
            AchievementProgress getAch = GetUsersAchievementProgress(user, achievementId);
            bool progress = getAch.Completed;

            if (progress) return true; else return false;
        }
        public async Task IncrementAchievementProgress(LoginUser user, int achievementId)
        {
            AchievementProgress getAch = GetUsersAchievementProgress(user, achievementId);
            getAch.UsersProgress += 1;
            await _context.SaveChangesAsync();
        }

        private void achievementDetails(AchievementProgress ach, LoginUser user, int userProgress, bool completed, int ratting)
        {
            //give the user the achievement
            //users progress
            ach.UsersProgress = userProgress;
            //when the user completed it
            ach.CompletedTime = DateTime.Now;
            //set it to true because its complete 
            ach.Completed = completed;
            //gives the user +1 to their ratting for the Achievement
            user.Ratting += ratting;
        }

        public async Task GiveFirstLoginAchievement(LoginUser user)
        {
            //give the user the achievement
            AchievementProgress ach = GetUsersAchievementProgress(user, 1);
            achievementDetails(ach, user, 1, true, 1);
            await _context.SaveChangesAsync();
        }

        public async Task GiveTenAchievement(LoginUser user)
        {
            AchievementProgress ach = GetUsersAchievementProgress(user, 3);
            achievementDetails(ach, user, 10, true, 1);
            await _context.SaveChangesAsync();
        }

        public int FollowAchievementProgress(LoginUser user)
        {
            //need to fix if there is no achievemtns 
            AchievementProgress ach = GetUsersAchievementProgress(user, 3);
            if (ach == null)
            {
                return 0;
            }
            return ach.UsersProgress;
        }

    }
}
