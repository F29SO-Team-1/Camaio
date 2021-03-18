using Login.Areas.Identity.Data;

namespace Login.Models
{
    public class ChannelMember
        {
            public int Id { get; set; }
            public Channel Channel { get; set; }
            public LoginUser User { get; set; }
        }
}