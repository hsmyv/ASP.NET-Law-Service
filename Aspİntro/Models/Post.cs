using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspİntro.Models
{
    public class Post
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int View { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<PostImage> Images { get; set; }
    }
}
