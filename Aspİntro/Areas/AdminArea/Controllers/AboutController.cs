using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostAboutVM aboutVM)
        {
            if (!ModelState.IsValid) return View();

            List<AboutImage> imageList = new List<AboutImage>();
            foreach (var photo in aboutVM.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);

                await photo.SaveFile(path);

                AboutImage aboutImage = new AboutImage
                {
                    Image = fileName
                };
                imageList.Add(aboutImage);
            }
            About about = new About
            {
                Title = aboutVM.Title,
                Description = aboutVM.Description,
                Content = aboutVM.Content,
                Images = imageList

            };
            await _context.AboutImages.AddRangeAsync(imageList);
            await _context.Abouts.AddAsync(about);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
