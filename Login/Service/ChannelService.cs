using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<LoginUser> _userManager;
        public ChannelService(ChannelContext context, TagContext tagContext, UserManager<LoginUser> userManager)
        {
            _context = context;
            _tagContext = tagContext;
            _userManager = userManager;
        }
        //Returns the list of user's channels
        public List<Channel> GetChannels(LoginUser user)
        {
            return _context.ChannelMember
                .Where(cm => cm.UserId == user.Id)
                .Select(cm => cm.Channel)
                .ToList();
        }
        //Get a channel by its title
        public Task<Channel> GetChannel(string title)
        {
            var channel = _context.Channels
                .FirstOrDefaultAsync(table => table.Title == title);
            return channel;
        }
        //Get a channel by id
        public Task<Channel> GetChannel(int id)
        {
            var channel = _context.Channels
                .FirstOrDefaultAsync(table => table.Id == id);
            return channel;
        }
        //Returns a joint table that has both the user and channel. Returns null if user is not the channel member
        public async Task<ChannelMember> GetChannelMember(LoginUser user, Channel channel)
        {
            var channelMember = await _context.ChannelMember
                .Where(table => table.Channel == channel)
                .Where(table => table.UserId == user.Id)
                .FirstOrDefaultAsync();
            return channelMember;
        }

        public void AddMember(Channel channel, LoginUser user)
        {
            var channelMember = new ChannelMember
            {
                Channel = channel,
                UserId = user.Id
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
        //Returns the list of all channel members
        public IEnumerable<LoginUser> GetChannelMembers(Channel channel)
        {
            var channelMembers = _context.ChannelMember
                .Where(table => table.Channel == channel)
                .Select(table => _userManager.FindByIdAsync(table.UserId).Result)
                .Distinct()
                .ToList();
            return channelMembers;
        }
        //Returns the list of all tags
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
        //Separates the tags into a list
        private List<Tag> GetTagList(Channel channel, string tags)
        {
            List<Tag> tagList = new List<Tag>();
            string tag = "";
            if (tags == null) return tagList; //If input is null, then return an empty list
            while (tags.Length != 0) //Loops through the entire input
            {
                if (tags.ElementAt(0).Equals((char)32) || tags.ElementAt(0).Equals((char)44)) //ASCII for whitespace and coma
                {
                    if (tag.Length > 1)
                    {
                        tagList.Add(new Tag
                        {  //Adds a tag to the list if it is 2 or more characters long
                            Name = tag,
                            Channel = channel
                        });
                    }
                    tag = ""; //reset the current tag
                    tags = tags.Substring(1); //remove the first input character
                }
                else
                {
                    tag += (tags.ElementAt(0)); //adds a character to the current keyword if it is not whitespace or coma
                    if (tags.Length == 1 && tag.Length > 1)
                    {
                        tagList.Add(new Tag
                        { //Adds a tag to the list if it is 2 or more characters long
                            Name = tag,
                            Channel = channel
                        });
                    }
                    tags = tags.Substring(1); //remove the first input character
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
        //Returns true if the channel is public. False otherwise
        public bool CheckIfPublic(Channel channel)
        {
            var isPublic = _context.Channels
                    .Where(table => table == channel)
                    .Select(table => table.Public)
                    .FirstOrDefault();
            return isPublic;
        }
        //Returns the list of all tags
        public IEnumerable<Tag> GetAllTags()
        {
            return _tagContext.Tags;
        }
        //Returns the list of all channels
        public IEnumerable<Channel> GetAll()
        {
            return _context.Channels;
        }
        public LoginUser GetByUserName(string username)
        {
            return _context.ChannelMember
                        .Where(u => u.User.UserName == username)
                        .Select(table => _userManager.FindByIdAsync(table.UserId).Result)
                        .FirstOrDefault();
        }
    }
}
