using Login.Areas.Identity.Data;

namespace Login.Models
{
    public class ChannelMember
        {
            public int Id { get; set; }
            public int ChannelId { get; set; }
            public Channel Channel { get; set; }
            public string UserName { get; set; }
            public LoginUser LoginUser { get; set; }
        }
}