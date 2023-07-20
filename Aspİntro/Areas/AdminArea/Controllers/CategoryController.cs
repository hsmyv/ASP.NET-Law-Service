using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Exceptions;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.AsNoTracking().ToListAsync();
            return View(categories); 
        }

        public IActionResult Detail(int id)
        {
            var category = _context.Categories.FirstOrDefault(m => m.Id == id);
            return View(category);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();  
            }
            bool isExist = _context.Categories.Where(m=>!m.IsDeleted).Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (isExist)
            {
                ModelState.AddModelError("Name", "This category already existed!");
                return View();
            }
            await _context.Categories.AddAsync(category);
            await  _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Categories.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id) return BadRequest();

            try
            {
              Category dbCategory = await _context.Categories.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
                        if (dbCategory.Name.ToLower().Trim() == category.Name.ToLower().Trim())
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        bool isExist = _context.Categories.Any(m => m.Name.ToLower().Trim() == category.Name.ToLower().Trim());
                        if (isExist)
                        {
                            ModelState.AddModelError("Name", "This category already existed!");
                            return View();
                        }
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
          

            //dbCategory.Name = category.Name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }  

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.Include(m=>m.Posts).Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (category == null) return NotFound();
            //_context.Categories.Remove(category);
            category.IsDeleted = true;
            foreach(var post in category.Posts)
            {
                post.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
