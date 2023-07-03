using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; } 
    }
}
