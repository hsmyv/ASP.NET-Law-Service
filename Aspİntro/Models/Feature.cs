using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class Feature: BaseEntity
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


        public string Image { get; set; }
    }
}
