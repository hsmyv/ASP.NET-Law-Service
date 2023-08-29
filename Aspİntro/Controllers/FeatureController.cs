using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Controllers
{
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync();
            if (feature is null) return NotFound();
            List<Review> reviews = await _context.Reviews.Where(m => !m.IsDeleted).ToListAsync();
            if (reviews is null) return NotFound();


            HomeVM homeVM = new HomeVM
            {
                Reviews = reviews,
                Feature = feature

            };

            return View(homeVM);
        }
    }
}
