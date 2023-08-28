using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.Models
{
    public class Review:BaseEntity
    {
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
