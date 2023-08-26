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
    public class AdvisorController : Controller
    {
        private readonly AppDbContext _context;

        public AdvisorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Advisor> advisors = await _context.Advisors.Where(m => !m.IsDeleted).ToListAsync();
            if (advisors is null) return NotFound();


            HomeVM homeVM = new HomeVM
            {
                Advisors = advisors,
            };

            return View(homeVM);
        }
    }
}
