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
    }
}
