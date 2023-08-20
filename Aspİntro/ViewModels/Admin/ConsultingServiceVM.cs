using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class ConsultingServiceVM
    {
        public string name { get; set; }
        public string description { get; set; }
        public IFormFile icon { get; set; }
    }
}
