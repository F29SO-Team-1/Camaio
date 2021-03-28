using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class ChannelService : IChannel
    {
        private readonly ChannelContext _context;
        private readonly TagContext _tagContext;
        public ChannelService(ChannelContext context, TagContext tagContext)
        {
            _context = context;
            _tagContext = tagContext;
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
            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<LoginUser> GetChannelMembers(Channel channel)
        {
            var channelMembers = _context.ChannelMember
                .Where(table => table.ChannelId == channel.Id)
                .Select(table => table.User)
                .Distinct()
                .ToList();
            return channelMembers;
        }
        public IEnumerable<Tag> GetChannelTags(Channel channel)
        {
            return _tagContext.Tags
                .Where(tag => tag.ChannelId == channel.Id)
                .Distinct()
                .ToList();
        }
        public void ChangeTags(Channel channel, string tags)
        {
            _context.RemoveRange(_tagContext.Tags.Where(tag => tag.ChannelId == channel.Id && tag.ChannelId != 0).Distinct().ToList());
            _context.AddRange(GetTagList(channel, tags));
            _context.SaveChanges();
        }
        private List<Tag> GetTagList(Channel channel, string tags)
        {
            List<Tag> tagList = new List<Tag>();
            string tag = "";
            if (tags==null) return tagList;
            while (tags.Length!=0)
            {
                if (tags.ElementAt(0).Equals((char)32) || tags.ElementAt(0).Equals((char)44))
                {
                    if(tag.Length>1)
                    {
                        tagList.Add( new Tag {
                            Name = tag,
                            Channel = channel
                        });
                    }
                    tag = "";
                    tags = tags.Substring(1);
                } 
                else
                {
                    tag+=(tags.ElementAt(0));
                    if (tags.Length==1 && tag.Length>1)
                    {
                        tagList.Add( new Tag {
                            Name = tag,
                            Channel = channel
                        });
                    }
                    tags = tags.Substring(1);
                }
            }
            return tagList;
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
        public IEnumerable<Tag> GetAllTags()
        {
            return _tagContext.Tags;
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
