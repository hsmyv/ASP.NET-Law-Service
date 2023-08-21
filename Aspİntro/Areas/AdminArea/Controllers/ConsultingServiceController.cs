using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Utilities.File;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class ConsultingServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ConsultingServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<ConsultingService> consultingServices = await _context.ConsultingServices.AsNoTracking().ToListAsync();
            return View(consultingServices);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultingServiceVM consultingServiceVM)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + consultingServiceVM.icon.FileName;
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await consultingServiceVM.icon.CopyToAsync(stream);
            }

            ConsultingService consultingService = new ConsultingService
            {
                name = consultingServiceVM.name,
                description = consultingServiceVM.description,
                icon = fileName
            };
            await _context.ConsultingServices.AddAsync(consultingService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var consultingService = await GetConsultingServiceById(id);
            if (consultingService is null) return NotFound();
            return View(consultingService);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsultingServiceVM consultingServiceVM)
        {
            var dbConsultingService = await GetConsultingServiceById(id);
            if (consultingServiceVM is null) return NotFound();

            /*
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!consultingService.icon.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (!consultingService.icon.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size is big than 100");
                return View();
            }*/

            string path = Helpers.GetFilePath(_env.WebRootPath, "img", dbConsultingService.icon);
            Helpers.DeleteFile(path);

            string fileName = Guid.NewGuid().ToString() + "_" + consultingServiceVM.icon.FileName;

            string newPath = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            using (FileStream stream = new FileStream(newPath, FileMode.Create))
            {
                await consultingServiceVM.icon.CopyToAsync(stream);
            }

            dbConsultingService.icon = fileName;
            dbConsultingService.name = consultingServiceVM.name;
            dbConsultingService.description = consultingServiceVM.description;
          
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<ConsultingService> GetConsultingServiceById(int id)
        {
            return await _context.ConsultingServices.FindAsync(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            ConsultingService consultingService = await GetConsultingServiceById(id);
            if (consultingService == null) return NotFound();
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", consultingService.icon);
            Helpers.DeleteFile(path);
            _context.ConsultingServices.Remove(consultingService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
