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

        public List<Channel> GetChannels(LoginUser user)
        {
            return _context.ChannelMember
                .Where(cm => cm.UserId == user.Id)
                .Select(cm => cm.Channel)
                .ToList();
        }

        public Task<Channel> GetChannel(string title)
        {
            var channel = _context.Channels
                .FirstOrDefaultAsync(table => table.Title == title);
            return channel;
        }
        public Task<Channel> GetChannel(int id)
        {
            var channel = _context.Channels
                .FirstOrDefaultAsync(table => table.Id == id);
            return channel;
        }

        public async Task<ChannelMember> GetChannelMember(LoginUser user, Channel channel)
        {
            var channelMember = await _context.ChannelMember
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
            _context.ChannelMember.Remove(channelMember);
            _context.SaveChanges();
        }

        public async Task DeleteChannel(Channel channel)
        {
            _context.ChannelMember.RemoveRange(_context.ChannelMember
                    .Where(table => table.Channel == channel)
                    .ToList());
            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
        }

        public List<string> GetChannelMembers(Channel channel)
        {
            var channelMembers = _context.ChannelMember
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
        public bool CheckIfPublic(Channel channel) 
        {
            var isPublic = _context.Channels
                    .Where(table => table == channel)
                    .Select(table => table.Public)
                    .FirstOrDefault();
            return isPublic;
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.Channels;
        }
        public LoginUser GetByUserName(string username)
        {
            return _context.ChannelMember
                        .Where(u => u.User.UserName == username)
                        .Select(table => table.User)
                        .FirstOrDefault();
        }
    }
}
