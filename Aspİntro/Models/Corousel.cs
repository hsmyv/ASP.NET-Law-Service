using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Aspİntro.Models
{
    public class Corousel:BaseEntity
    {
        [Required]
        public string Header { get; set; }
        public string Description { get; set; }
        public string GetStartedURL { get; set; }
        public string WatchVideoURL { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
}
