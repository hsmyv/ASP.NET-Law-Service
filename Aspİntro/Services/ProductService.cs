using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetPosts(int take )
        {
            try
            {
                IEnumerable<Post> posts = await _context.Posts.Where(x => x.IsDeleted == false)
               .OrderByDescending(m => m.Id)
               .Take(take)
               .ToListAsync();
                return posts;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
