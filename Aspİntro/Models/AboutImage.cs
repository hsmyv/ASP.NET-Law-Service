using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class AboutImage: BaseEntity
    {
        public string Image { get; set; }
        public int AboutId { get; set; }
        public About About { get; set; }

    }
}
