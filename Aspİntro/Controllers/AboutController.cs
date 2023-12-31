﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Feature feature = await _context.Features.FirstOrDefaultAsync();
            if (feature is null) return NotFound();

            About about = await _context.Abouts.Include(m => m.Images).FirstOrDefaultAsync();
            if (about is null) return NotFound();

            HomeVM homeVM = new HomeVM
            {
                About = about,
                Feature = feature

            };

            return View(homeVM);
        }
    }
}
