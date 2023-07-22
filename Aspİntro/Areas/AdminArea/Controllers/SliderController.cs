using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvirment _env;

        public SliderController(AppDbContext context, IWebHostEnviroment env)
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
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!slider.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }
            if (slider.Photo.Length / 1024 > 100)
            {
                ModelState.AddModelError("Photo", "Image size is big than 100");
                return View();
            }
            string filename = Guid.NewGuid().ToString() + "_" + slider.Photo.FileName;
            string path = @"D:\Hasan\C# projects\AspNet\Aspİntro\Aspİntro\wwwroot\img\" + filename;

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }
            return View();
        }
    }

    internal interface IWebHostEnvirment
    {
    }
}
