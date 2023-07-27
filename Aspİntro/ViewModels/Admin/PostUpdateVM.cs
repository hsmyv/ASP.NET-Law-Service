using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.ViewModels.Admin
{
    public class PostUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public ICollection<PostImage> Images { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
