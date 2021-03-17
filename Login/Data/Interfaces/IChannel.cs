using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;

namespace Login.Data
{
    public interface IChannel //Copyright Apple Inc.
    {
        List<string> GetChannels(string user);
        Task<Channel> GetChannel(string id);
        Task<ChannelMember> GetChannelMember(string user, Channel channel);
        void AddMember(Channel channel, string userName);
        void RemoveMember(ChannelMember channelMember);
        Task DeleteChannel(Channel channel);
        List<string> GetChannelMembers(Channel channel);
        Task UpdateChannel(Channel channel, string description);
        void CreateChannel(Channel channel);

        IEnumerable<Channel> UserChannel(string userName);
        IEnumerable<Channel> GetAll();
    }
}
