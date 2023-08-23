using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class FeatureCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string label1 { get; set; }
        public int label1Count { get; set; }

        public string label2 { get; set; }
        public int label2Count { get; set; }

        public string label3 { get; set; }
        public int label3Count { get; set; }

        public string label4 { get; set; }
        public int label4Count { get; set; }


        public IFormFile Image { get; set; }
    }
}
