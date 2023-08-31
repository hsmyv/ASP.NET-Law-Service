using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.ViewComponents
{
    public class FeatureViewComponent: ViewComponent
    {
        private readonly AppDbContext _context;

        public FeatureViewComponent(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync();

            return (await Task.FromResult(View(feature)));
        }
    }
}
