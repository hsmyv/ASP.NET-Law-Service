using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class SliderVM
    {
        public int Id { get; set; }

        [NotMapped, Required]
        public List<IFormFile> Photos { get; set; }
    }
}
