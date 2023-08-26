using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class Advisor: BaseEntity
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
