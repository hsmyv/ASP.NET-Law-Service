using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Aspİntro.ViewComponents
{
    public class ConsultingServiceViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ConsultingServiceViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var consultingServices = await _context.ConsultingServices.ToListAsync();
            return (await Task.FromResult(View(consultingServices)));
        }
    }
}
