using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PostCount = _context.Posts.Where(p => p.IsDeleted == false).Count();
            List<Post> post = await _context.Posts.Where(x => x.IsDeleted == false)
                .OrderByDescending(m=> m.Id)
                .Take(2)
                .ToListAsync();  
            return View(post);
        }

        public IActionResult LoadMore(int skip)
        {
            List<Post> posts = _context.Posts.Where(x => x.IsDeleted == false)
               .OrderByDescending(m=> m.Id)
               .Skip(skip)
               .Take(2)
               .ToList();
            return PartialView("_PostsPartial", posts);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return NotFound();
            Post dbPost = await _context.Posts.FindAsync(id);
            if (dbPost == null) return BadRequest();
            return Json(id);
        }
         

    }
}
