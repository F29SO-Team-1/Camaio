using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.Models;

namespace Login.Data
{
    public interface IChannel //Copyright Apple Inc.
    {
        Task CreateChannel();
        Task DeleteChannel();
        Task JoinChannel();
        Task LeaveChannelP();
        Task ChangeSettings();


        IEnumerable<Channel> UserChannel(string userName);
        IEnumerable<Channel> GetAll();
    }
}
