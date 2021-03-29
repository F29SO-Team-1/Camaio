using Login.Data;
using Login.Data.Interfaces;
using System.Threading.Tasks;

namespace Login.Service
{
    public class TagService : ITag
    {
        private readonly TagContext _context;
        public TagService(TagContext context)
        {
            _context = context;
        }

        public Task CreateTag()
        {
            throw new System.NotImplementedException();
        }
    }
}
