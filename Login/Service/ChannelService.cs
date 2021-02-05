using Login.Data;
using Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Service
{
    public class ChannelService : IChannel
    {
        private readonly ChannelContext _context;
        public ChannelService(ChannelContext context)
        {
            _context = context;
        }
        public Task ChangeSettings()
        {
            throw new NotImplementedException();
        }

        public Task CreateChannel()
        {
            throw new NotImplementedException();
        }

        public Task DeleteChannel()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Channel> GetAll()
        {
            return _context.Channel;
        }

        public Task JoinChannel()
        {
            throw new NotImplementedException();
        }

        public Task LeaveChannelP()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Channel> UserChannel(string userName)
        {
            return GetAll().Where(c => c.Creator == userName);
        }
    }
}
