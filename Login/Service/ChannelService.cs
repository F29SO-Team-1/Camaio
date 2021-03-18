using Login.Data;
using Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Login.Areas.Identity.Data;

namespace Login.Service
{
    public class ChannelService : IChannel
    {
        private readonly ChannelContext _context;
        public ChannelService(ChannelContext context)
        {
            _context = context;
        }

        public List<string> GetChannels(LoginUser user)
        {
            var channelList = _context.ChannelMembers
                .Where(table => table.User == user)
                .Join(
                    _context.Channel,
                    channelMembers => channelMembers.Channel,
                    channel => channel,
                    (channelMember, channel) => channel.Title
                )
                .ToList();
            return channelList;
        }

        public Task<Channel> GetChannel(string id)
        {
            var channel = _context.Channel
                .FirstOrDefaultAsync(table => table.Title == id);
            return channel;
        }

        public async Task<ChannelMember> GetChannelMember(LoginUser user, Channel channel)
        {
            var channelMember = await _context.ChannelMembers
                .Where(table => table.Channel == channel)
                .Where(table => table.User == user)
                .FirstOrDefaultAsync();
            return channelMember;
        }

        public void AddMember(Channel channel, LoginUser user)
        {
            var channelMember = new ChannelMember 
            {
                Channel = channel,
                User = user
            };
            _context.Add(channelMember);
            _context.SaveChanges();
        }

        public void RemoveMember(ChannelMember channelMember)
        {
            _context.ChannelMembers.Remove(channelMember);
            _context.SaveChanges();
        }

        public async Task DeleteChannel(Channel channel)
        {
            _context.ChannelMembers.RemoveRange(_context.ChannelMembers
                    .Where(table => table.Channel == channel)
                    .ToList());
            _context.Channel.Remove(channel);
            await _context.SaveChangesAsync();
        }

        public List<string> GetChannelMembers(Channel channel)
        {
            var channelMembers = _context.ChannelMembers
                .Where(table => table.Channel == channel)
                .Select(table => table.User.UserName)
                .ToList();
            return channelMembers;
        }

        public async Task UpdateChannel(Channel channel, string description)
        {
            channel.Description = description;
            await _context.SaveChangesAsync();
        }

        public void CreateChannel(Channel channel)
        {
            _context.Add(channel);
            _context.SaveChanges();
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.Channel;
        }

        public IEnumerable<Channel> UserChannel(string userName)
        {
            return GetAll().Where(c => c.Creator.UserName == userName);
        }
        public LoginUser GetByUserName(string username)
        {
            return _context.ChannelMembers
                        .Where(u => u.User.UserName == username)
                        .Select(table => table.User)
                        .FirstOrDefault();
        }
    }
}
