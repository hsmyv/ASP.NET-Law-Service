using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;

namespace Aspİntro.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Post> Posts { get; set; }
        public SliderDetail Detail { get; set; }

        public List<ConsultingService> ConsultingServices { get; set; }

        public About About { get; set; }
        public Feature Feature { get; set; }
        public List<Advisor> Advisors { get; set; }
    }
}
