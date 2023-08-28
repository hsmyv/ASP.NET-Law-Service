using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class ReviewUpdateVM
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public bool IsActive { get; set; }
    }
}
