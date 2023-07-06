using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aspİntro.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PostCount = _context.Posts.Where(p => p.IsDeleted == false).Count();
            List<Post> post = await _context.Posts.Where(x => x.IsDeleted == false)
                .OrderByDescending(m=> m.Id)
                .Take(2)
                .ToListAsync();  
            return View(post);
        }

        public IActionResult LoadMore(int skip)
        {
            List<Post> posts = _context.Posts.Where(x => x.IsDeleted == false)
               .OrderByDescending(m=> m.Id)
               .Skip(skip)
               .Take(2)
               .ToList();
            return PartialView("_PostsPartial", posts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) return NotFound();
            Post dbPost = await GetPostById(id);
            if (dbPost == null) return BadRequest();

            List<BasketVM> basket = GetBasket();
            UpdateBasket(basket, dbPost);
         
            return RedirectToAction("Index", "Home");
        }
        private async Task<Post> GetPostById(int? id)
        {
            return await _context.Posts.FindAsync(id);
        }
        private void UpdateBasket(List<BasketVM> basket, Post post)
        {
            var existPost = basket.Find(m => m.Id == post.Id);

            if (existPost == null)
            {
                basket.Add(new BasketVM
                {
                    Id = post.Id,
                    Count = 1
                });
            }
            else
            {
                existPost.Count++;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));


        }
        private List<BasketVM> GetBasket()
        {
            List<BasketVM> basket;
            if(Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }
            return basket;
        }
        public async Task<IActionResult> Basket()
        {
            List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            List<BasketDetailVM> basketDetailItems = new List<BasketDetailVM>();
            foreach (BasketVM item in basket)
            {
                Post post = await _context.Posts.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == item.Id);
                BasketDetailVM basketDetail = new BasketDetailVM
                {
                    Id = item.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Count  = item.Count,
                    Image = post.Images.Where(m => m.IsMain).FirstOrDefault().Image,
                    
                };
                basketDetailItems.Add(basketDetail);
            }

            return View(basketDetailItems);
        }
         
    }
}
