using Login.Areas.Identity.Data;
using Login.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Login.Data
{
    public interface IChannel //Copyright Apple Inc.
    {
        List<Channel> GetChannels(LoginUser user);
        Task<Channel> GetChannel(string title);
        Task<Channel> GetChannel(int id);
        Task<ChannelMember> GetChannelMember(LoginUser user, Channel channel);
        void AddMember(Channel channel, LoginUser userName);
        void RemoveMember(ChannelMember channelMember);
        Task DeleteChannel(Channel channel);
        IEnumerable<LoginUser> GetChannelMembers(Channel channel);
        IEnumerable<Tag> GetChannelTags(Channel channel);
        Task UpdateChannel(Channel channel, string description);
        void CreateChannel(Channel channel);
        bool CheckIfPublic(Channel channel);
        void ChangeTags(Channel channel, string tags);
        IEnumerable<Channel> GetAll();
        LoginUser GetByUserName(string username);
    }
}
