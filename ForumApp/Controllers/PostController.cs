using ForumApp.Data;
using ForumApp.Data.Entities;
using ForumApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace ForumApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly ForumAppDbContext data;

        public PostsController(ForumAppDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> All()
        {
            var posts = await data.Posts
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                })
                .ToListAsync();

            return View(posts);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Add(PostFormModel model)
        {
            var post = new Post
            {

                Title = model.Title,
                Content = model.Content

            };

            await data.Posts.AddAsync(post);
            await data.SaveChangesAsync();

            return RedirectToAction("All");

        }

        public async Task<IActionResult> Edit(int id)

        { 
            var post = await data.Posts.FindAsync(id);

            return View(new PostFormModel() 
            { 
                Title = post.Title,
                Content= post.Content,
            });
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, PostViewModel model)
        {
            var post = await data.Posts.FindAsync(id);
            post.Title = model.Title;
            post.Content = model.Content;
            
            await data.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await data.Posts.FindAsync(id);

            data.Posts.Remove(post);

            await data.SaveChangesAsync();

            return RedirectToAction("All");
        }
    }
}
