using Login.Areas.Identity.Data;


namespace Login.Models
{
    public class WaitingToJoin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public LoginUser User { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

    }
}