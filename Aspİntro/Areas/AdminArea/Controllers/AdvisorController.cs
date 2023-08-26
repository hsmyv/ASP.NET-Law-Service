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
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AdvisorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public AdvisorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Advisor> advisors = await _context.Advisors.Where(m => !m.IsActive).AsNoTracking().ToListAsync();
            return View(advisors);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Advisor advisor = await _context.Advisors.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (advisor == null) return NotFound();

            return View(advisor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdvisorCreateVM advisorVM)
        {
           
            if (!ModelState.IsValid) return View();

            string fileName = Guid.NewGuid().ToString() + "_" + advisorVM.Image.FileName;
            string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
            await advisorVM.Image.SaveFile(path);



            Advisor advisor = new Advisor
            {
                Name = advisorVM.Name,
                Profession = advisorVM.Profession,
                FacebookUrl = advisorVM.FacebookUrl,
                LinkedinUrl = advisorVM.LinkedinUrl,
                TwitterUrl = advisorVM.TwitterUrl,
                InstagramUrl = advisorVM.InstagramUrl,             
            };
            advisor.Image = fileName;

            await _context.Advisors.AddAsync(advisor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdvisorUpdateVM advisorUpdateVM)
        {
          
            Advisor advisor = await _context.Advisors.AsNoTracking().Where(m => !m.IsDeleted && m.Id == id && !m.IsActive).FirstOrDefaultAsync();
            if (advisor is null) return NotFound();

            
            if(advisorUpdateVM.Image != null)
            {
                string path = Helpers.GetFilePath(_env.WebRootPath, "img", advisor.Image);
                Helpers.DeleteFile(path);

                string fileName = Guid.NewGuid().ToString() + "_" + advisorUpdateVM.Image.FileName;
               string newPath = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);
               await advisorUpdateVM.Image.SaveFile(newPath);

               advisor.Image = fileName;

            }


            advisor.Name = advisorUpdateVM.Name;
            advisor.Profession = advisorUpdateVM.Profession;
            advisor.InstagramUrl = advisorUpdateVM.InstagramUrl;
            advisor.TwitterUrl = advisorUpdateVM.TwitterUrl;
            advisor.FacebookUrl = advisorUpdateVM.FacebookUrl;
            advisor.LinkedinUrl = advisorUpdateVM.LinkedinUrl;

            _context.Advisors.Update(advisor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Advisor advisor = await _context.Advisors.Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (advisor == null) return NotFound();
            advisor.IsDeleted = true;
          
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
