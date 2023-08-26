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
    public class ConsultingServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultingServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ConsultingService> consultingServices = await _context.ConsultingServices.ToListAsync();
            if (consultingServices is null) return NotFound();
            Feature feature = await _context.Features.FirstOrDefaultAsync();
            if (feature is null) return NotFound();


            HomeVM homeVM = new HomeVM
            {
                ConsultingServices = consultingServices,
                Feature = feature

            };

            return View(homeVM);
        }
    }
}
