using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class PostCreateVM
    {
        public int Id{ get; set; }
        public string  Title { get; set; }
        public string  Description { get; set; }
        public decimal CategoryId { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
