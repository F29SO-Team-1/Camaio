using Login.Areas.Identity.Data;

namespace Login.Models
{
    public class Following
    {
        public int Id { get; set; }
        public string Username { get; set; } //user follows

        //fkey in LoginUser
        public LoginUser FollowingUsers { get; set; } //which user they are following
    }
}
