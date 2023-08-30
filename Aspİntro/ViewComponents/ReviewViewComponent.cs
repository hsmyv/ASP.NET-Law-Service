using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Aspİntro.ViewComponents
{
    public class ReviewViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ReviewViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return (await Task.FromResult(View(reviews)));
        }
    }
}
