using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class PostImage:BaseEntity
    {
        public string Image { get; set; }
        public int PostId { get; set; }
        public bool IsMain { get; set; } = false;
        public Post Post { get; set; }
    }
}
