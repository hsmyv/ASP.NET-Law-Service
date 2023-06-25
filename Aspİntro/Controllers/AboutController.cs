using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aspİntro.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            Product product = new Product
            {
                Id = 1,
                Name = "BMW"
            };
            return View(product);
        }
    }
}
