using Login.Data;
using Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Login.Service
{
    public class ChannelService : IChannel
    {
        private readonly ChannelContext _context;
        public ChannelService(ChannelContext context)
        {
            _context = context;
        }

        public List<string> GetChannels(string user)
        {
            var channelList = _context.ChannelMembers
                .Where(table => table.UserName == user)
                .Join(
                    _context.Channel,
                    channelMembers => channelMembers.ChannelId,
                    channel => channel.Id,
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

        public async Task<ChannelMember> GetChannelMember(string userName, Channel channel)
        {
            var channelMember = await _context.ChannelMembers
                .Where(table => table.ChannelId == channel.Id)
                .Where(table => table.UserName == userName)
                .FirstOrDefaultAsync();
            return channelMember;
        }

        public void AddMember(Channel channel, string userName)
        {
            var channelMember = new ChannelMember 
            {
                ChannelId = channel.Id,
                UserName = userName
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
                    .Where(table => table.ChannelId == channel.Id)
                    .ToList());
            _context.Channel.Remove(channel);
            await _context.SaveChangesAsync();
        }

        public List<string> GetChannelMembers(Channel channel)
        {
            var channelMembers = _context.ChannelMembers
                .Where(table => table.ChannelId == channel.Id)
                .Select(table => table.UserName)
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
            return GetAll().Where(c => c.Creator == userName);
        }
    }
}
