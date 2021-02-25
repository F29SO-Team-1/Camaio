using Login.Areas.Identity.Data;

namespace Login.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Thread Thread { get; set; }
    }
}