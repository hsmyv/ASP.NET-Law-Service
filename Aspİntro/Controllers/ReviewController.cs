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
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Review> reviews = await _context.Reviews.Where(m => !m.IsDeleted && m.IsActive).ToListAsync();
            if (reviews is null) return NotFound();

           

            HomeVM homeVM = new HomeVM
            {
                Reviews = reviews,
            };

            return View(homeVM);
        }
    }
}
