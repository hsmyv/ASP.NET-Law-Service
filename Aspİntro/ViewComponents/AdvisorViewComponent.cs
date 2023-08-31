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
    public class AdvisorViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public AdvisorViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var advisors = await _context.Advisors.ToListAsync();
            return (await Task.FromResult(View(advisors)));
        }
    }
}
