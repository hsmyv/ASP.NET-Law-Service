using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(m => m.Category)
                .Include(m => m.Images)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            var result = GetMapDatas(posts);
            return View();
        }

        private List<PostListVM> GetMapDatas(List<Post> posts)
        {
            List<PostListVM> postList = new List<PostListVM>();
            foreach (var post in posts)
            {
                PostListVM newPost = new PostListVM
                {
                    Id = post.Id,
                    Name = post.Title,
                    Image = post.Images.Where(m=>m.IsMain).FirstOrDefault()?.Image,
                    CategoryName = post.Category.Name
                };

                PostListVM.Add(newPost);
            }
            return postList;
        }
    }
}
