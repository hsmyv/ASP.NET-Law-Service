 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //HttpContext.Session.SetString("name", "Hasan");

            List<Slider> sliders = await _context.Sliders.ToListAsync();
            List<ConsultingService> consultingServices = await _context.ConsultingServices.ToListAsync();

            List<Post> posts = await _context.Posts.Include(x=>x.Images).Include(x=>x.Category).ToListAsync();
            List<Review> reviews = await _context.Reviews.Where(m => m.IsActive).AsNoTracking().ToListAsync();
            Feature feature = await _context.Features.FirstOrDefaultAsync();

            About about = await _context.Abouts.Include(x => x.Images).FirstOrDefaultAsync();
            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                Posts  = posts,
                ConsultingServices = consultingServices,
                About = about,
                Reviews = reviews,
                Feature = feature

            };
 
            return View(homeVM); 
        }

        public IActionResult Test()
        {
            var session = HttpContext.Session.GetString("name");
            if (session == null) return NotFound();
            return Json(session);
        }
    }
}
