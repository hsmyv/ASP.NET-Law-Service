using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Admin;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.AsNoTracking().ToListAsync();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View(); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {
            #region for single file
            if (ModelState["Image"].ValidationState == ModelValidationState.Invalid) return View();
            if (!sliderVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Image type is wrong");
                return View();
            }
            if (!sliderVM.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size is big than 100");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + sliderVM.Image.FileName;
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            await sliderVM.Image.SaveFile(path);

            Slider slider = new Slider
            {
                Header = sliderVM.Header,
                Description = sliderVM.Description,
                GetStartedURL = sliderVM.GetStartedURL,
                WatchVideoURL = sliderVM.WatchVideoURL,
                Image = fileName,
                IsActive = true,

            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            #endregion
            //if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            //foreach (var photo in sliderVM.Photos)
            //{
            //    if (!photo.CheckFileType("image/"))
            //    {
            //        ModelState.AddModelError("Photo", "Image type is wrong");
            //        return View();
            //    }
            //    if (!photo.CheckFileSize(200))
            //    {
            //        ModelState.AddModelError("Photo", "Image size is big than 100");
            //        return View();
            //    }
            //}
            //foreach (var photo in sliderVM.Photos)
            //{
            //    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            //    string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);

            //    using (FileStream stream = new FileStream(path, FileMode.Create))
            //    {
            //        await photo.CopyToAsync(stream);
            //    }

            //    Slider slider = new Slider
            //    {
            //        Image = fileName
            //    };
            //    await _context.Sliders.AddAsync(slider);
            //    await _context.SaveChangesAsync();
            //}
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            Slider slider = await GetSliderById(id);
            if (slider == null) return NotFound();
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", slider.Image);
            Helpers.DeleteFile(path);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<Slider> GetSliderById(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var slider = await GetSliderById(id);
            if (slider is null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SliderVM sliderVM)
        {
            var dbSlider = await GetSliderById(id);
            if (sliderVM is null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!sliderVM.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (!sliderVM.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size is big than 100");
                return View();
            }
            
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", dbSlider.Image);
            Helpers.DeleteFile(path);

            string fileName = Guid.NewGuid().ToString() + "_" + sliderVM.Image.FileName;

            string newPath = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            using(FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await sliderVM.Image.CopyToAsync(stream);
            }

            dbSlider.Image = fileName;

            //slider.Image = fileName;
            //_context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

   
}
