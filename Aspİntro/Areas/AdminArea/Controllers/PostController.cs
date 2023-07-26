using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Utilities.Pagination;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index(int page  = 1, int take = 10)
        {
            var posts = await _context.Posts
                .Include(m => m.Category)
                .Include(m => m.Images)
                .Skip((page - 1)*take)
                .Take(take)
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .ToListAsync();
            var postsVM = GetMapDatas(posts);
            int count = await GetPageCount(take);

            Paginate<PostListVM> result = new Paginate<PostListVM>(postsVM, page, count);
            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            var count = await _context.Posts.CountAsync();
            return (int)Math.Ceiling((decimal)count / take);
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

                postList.Add(newPost);
            }
            return postList;
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Where(m=> !m.IsDeleted).ToListAsync();
            ViewBag.categories = new SelectList(categories, "id", "name");
            return View();
        }
    }
}
