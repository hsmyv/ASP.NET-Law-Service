using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspİntro.Data;
using Aspİntro.Models;
using Aspİntro.Utilities.File;
using Aspİntro.Utilities.Pagination;
using Aspİntro.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Aspİntro.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PostController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page  = 1, int take = 10)
        {
            var posts = await _context.Posts
                .Where(m=> !m.IsDeleted)
                .Include(m => m.Category)
                .Include(m => m.Images)
                .Skip((page - 1)*take)
                .Take(take)
                .OrderByDescending(m => m.Id)
                .AsNoTracking()
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
            ViewBag.categories = await GetCategoriesByPost();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateVM postVM)
        {
            ViewBag.categories = await GetCategoriesByPost();
            if (!ModelState.IsValid) return View();

            List<PostImage> imageList = new List<PostImage>();
            foreach (var photo in postVM.Photos)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);

                await photo.SaveFile(path);

                PostImage postImage = new PostImage
                {
                    Image = fileName
                };
                imageList.Add(postImage);
            }
            Post post = new Post
            {
                Title = postVM.Title,
                Description = postVM.Description,
                CategoryId = postVM.CategoryId,
                Images = imageList

            };
            await _context.PostImages.AddRangeAsync(imageList);
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Post post = await _context.Posts.Include(m=>m.Images).Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (post is null) return NotFound();

            foreach (var item in post.Images)
            {
                string path = Helpers.GetFilePath(_env.WebRootPath, "img", item.Image);
                Helpers.DeleteFile(path);
                item.IsDeleted = true;
            }

            post.IsDeleted = true;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            

        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.categories = await GetCategoriesByPost();

            Post post = await _context.Posts.Include(m => m.Images).Include(m=>m.Category).Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (post is null) return NotFound();

            PostUpdateVM result = new PostUpdateVM
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                CategoryId = post.CategoryId,
                Images = post.Images
            };            
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostUpdateVM postUpdateVM)
        {
            ViewBag.categories = await GetCategoriesByPost();
            if (!ModelState.IsValid) return View(postUpdateVM);
            Post post = await _context.Posts.Include(m => m.Images).Include(m => m.Category).Where(m => !m.IsDeleted && m.Id == id).FirstOrDefaultAsync();
            if (post is null) return NotFound();
            List<PostImage> imageList = new List<PostImage>();

            if(postUpdateVM.Photos != null)
            {
                foreach (var item in post.Images)
                {
                    string path = Helpers.GetFilePath(_env.WebRootPath, "img", item.Image);
                    Helpers.DeleteFile(path);
                    item.IsDeleted = true;
                }
                foreach (var photo in postUpdateVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string path = Helpers.GetFilePath(_env.WebRootPath, "img", fileName);

                    await photo.SaveFile(path);

                    PostImage postImage = new PostImage
                    {
                        Image = fileName
                    };
                    imageList.Add(postImage);
                }

                imageList.FirstOrDefault().IsMain = true;
                post.Images = imageList;

            }

            post.Title = postUpdateVM.Title;
            post.Description = postUpdateVM.Description;
            post.CategoryId = postUpdateVM.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> SetDefaultImage(DefaultImageVM model)
        {
            List<PostImage> postImages = await _context.PostImages.Where(m => m.PostId == model.PostId).ToListAsync();
            foreach (var image in postImages)
            {

                if(image.Id == model.ImageId)
                {
                    image.IsMain = true;
                }
                 else
                {
                    image.IsMain = false;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(postImages);
        }
        public async Task<SelectList> GetCategoriesByPost()
        {
            var categories = await _context.Categories.Where(m=> !m.IsDeleted).ToListAsync();
            return new SelectList(categories, "Id", "Name");
  
        }
    }
}
