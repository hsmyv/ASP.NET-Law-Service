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

    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FeatureController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeatureCreateVM featureVM)
        {
            var existingRecords = await _context.Features.FirstOrDefaultAsync();
            if (existingRecords != null)
            {
                string deletedPath = Helpers.GetFilePath(_env.WebRootPath, "img", existingRecords.Image);
                Helpers.DeleteFile(deletedPath);
                _context.Features.RemoveRange(existingRecords);
            }
                

            if (!ModelState.IsValid) return View();

            string fileName = Guid.NewGuid().ToString() + "_" + featureVM.Image.FileName;
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            await featureVM.Image.SaveFile(path);


            
            Feature feature = new Feature
            {
                Title = featureVM.Title,
                Description = featureVM.Description,
                label1 = featureVM.label1,
                label1Count = featureVM.label1Count,
                label2 = featureVM.label2,
                label2Count = featureVM.label2Count,
                label3 = featureVM.label3,
                label3Count = featureVM.label3Count,
                label4 = featureVM.label4,
                label4Count = featureVM.label4Count,
            };
            feature.Image = fileName;
       
            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




    }
}
