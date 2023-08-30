using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.AsNoTracking().ToListAsync();
            return View(settings);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSettingVM settingVM)
        {
            
            Setting setting = new Setting
            {
                Key = settingVM.Key,
                Value = settingVM.Value,
          

           };

            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
     
           
            return RedirectToAction(nameof(Index));
        }

        private async Task<Setting> GetSettingById(int id)
        {
            return await _context.Settings.FindAsync(id);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var setting = await GetSettingById(id);
            if (setting is null) return NotFound();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Setting setting)
        {
            var dbSetting = await GetSettingById(id);
            if (dbSetting is null) return NotFound();

            dbSetting.Key = setting.Key;
            dbSetting.Value = setting.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var setting = await GetSettingById(id);
            if (setting is null) return NotFound();

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
