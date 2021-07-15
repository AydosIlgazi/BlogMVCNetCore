using BlogMVC.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Controllers
{
    public class BlogController : Controller
    {
        private readonly IRepository _repo;
        public BlogController(IRepository repo)
        {
            _repo = repo;
        }
        [Route("blog/{title?}/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {

            var blogPost = await _repo.FindBlogPostAsync(id);
            return View(blogPost);
        }
    }
}
