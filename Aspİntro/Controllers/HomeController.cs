 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Models;
using Aspİntro.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Aspİntro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();

            Student st1 = new Student
            {
                Id = 1,
                Name = "Esger",
                Surname = "Esgerov",
                Age = 26
            };
            Student st2 = new Student
            {
                Id = 2,
                Name = "Hasan",
                Surname = "Musa",
                Age = 23  
            };

            students.Add(st1);
            students.Add(st2);

            Product product = new Product
            {
                Id = 1,
                Name = "Mercedes"
            };

            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6};

            HomeViewModel homeViewModel = new HomeViewModel
            {
                Students = students,
                Product = product ,
                Counts = list

            };
            return View(homeViewModel);
        }
    }
}
