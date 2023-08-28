using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReviewController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Review> reviews = await _context.Reviews.Where(m => m.IsActive).AsNoTracking().ToListAsync();
            return View(reviews);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            Review review = await _context.Reviews.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (review == null) return NotFound();

            return View(review);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewCreateVM reviewVM)
        {
           
            if (!ModelState.IsValid) return View();

            string fileName = Guid.NewGuid().ToString() + "_" + reviewVM.Image.FileName;
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            await reviewVM.Image.SaveFile(path);



            Review review = new Review
            {
                Name = reviewVM.Name,
                Profession = reviewVM.Profession,
                Content = reviewVM.Content,
                IsActive = true,

            };
            review.Image = fileName;
       
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReviewUpdateVM reviewUpdateVM)
        {

            Review review = await _context.Reviews.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id && !m.IsActive).FirstOrDefaultAsync();
            if (review is null) return NotFound();


            if (reviewUpdateVM.Image != null)
            {
                string path = Helpers.GetFilePath(_env.WebRootPath, "img", review.Image);
                Helpers.DeleteFile(path);

                string fileName = Guid.NewGuid().ToString() + "_" + reviewUpdateVM.Image.FileName;
                string newPath = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
                await reviewUpdateVM.Image.SaveFile(newPath);

                review.Image = fileName;

            }


            review.Name = reviewUpdateVM.Name;
            review.Profession = reviewUpdateVM.Profession;
            review.Content = reviewUpdateVM.Content;
         

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Review review = await _context.Reviews.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (review == null) return NotFound();
            review.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




    }
}
