using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;

namespace Aspİntro.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.AsEnumerable().ToDictionary(m => m.Key, m => m.Value);
        }
    }
}
