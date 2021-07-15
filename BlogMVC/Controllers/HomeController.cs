using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;

        public HomeController(ILogger<HomeController> logger,IRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _repo.GetAllBlogPostsAsync();
            var categories = await _repo.GetAllCategoriesAsync();
            ViewBag.CategoryList = categories;
            return View(blogPosts);
        }
        [Route("Category/{categoryId}")]
        public async Task<IActionResult> Category(int categoryId)
        {
            var blogPosts = await _repo.GetAllBlogPostsByCategoryAsync(categoryId);
            var categories = await _repo.GetAllCategoriesAsync();
            ViewBag.CategoryList = categories;
            return View("Index",blogPosts);
        }


    }
}
