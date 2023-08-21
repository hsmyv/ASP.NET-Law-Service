using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class About: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public ICollection<AboutImage> Images { get; set; }
    }
}
