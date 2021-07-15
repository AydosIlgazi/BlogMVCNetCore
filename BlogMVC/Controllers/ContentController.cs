using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Controllers
{
    [Authorize(Roles = "contentmanager")]
    public class ContentController : Controller
    {
        private readonly IRepository _repo; 
        public ContentController(IRepository repo)
        {
            _repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _repo.GetAllBlogPostsWithCategoryAsync();
            return View(blogPosts);
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload.Length <= 0) return null;

            //your custom code logic here

            //1)check if the file is image

            //2)check if the file is too large

            //etc

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            //save file under wwwroot/CKEditorImages folder

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/CKEditorImages",
                fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                upload.CopyTo(stream);
            }

            var url = $"{"/CKEditorImages/"}{fileName}";

            var successMessage = "image is uploaded";

            //dynamic success = Newtonsoft.Json.JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            //return Json(success);
            return Ok(new {uploaded=1 ,fileName=fileName,url=url });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                IFormFile file = Request.Form.Files["Image"];
                category.Image = GetByteArrayFromImage(file);
                await _repo.AddCategoryAsync(category);
                return RedirectToAction("Index");
            }
            return View("AddCategory",category);
        }
        
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _repo.FindCategoryAsync(id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await _repo.EditCategoryAsync(category);
                return RedirectToAction("Index");
            }
            return View("EditCategory", category);
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            var category = await _repo.FindCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _repo.DeleteCategoryAsync(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddBlogPost()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            categories.Insert(0, new Category { Id = 0, Name = "Select" });
            ViewBag.CategoryList = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBlogPost(BlogPost blogPost)
        {
            blogPost.PostDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                await _repo.AddBlogPostAsync(blogPost);
                return RedirectToAction("Index");
            }
            return View("AddBlogPost", blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBlogPost(int? id)
        {
            var blogPost = await _repo.FindBlogPostAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            await _repo.DeleteBlogPostAsync(blogPost);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditBlogPost(int id)
        {
            var categories = await _repo.GetAllCategoriesAsync();
            categories.Insert(0, new Category { Id = 0, Name = "Select" });
            ViewBag.CategoryList = categories;
            var blogPost = await _repo.FindBlogPostAsync(id);
            return View(blogPost);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlogPost(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                await _repo.EditBlogPostAsync(blogPost);
                return RedirectToAction("Index");
            }
            return View("EditBlogPost", blogPost);
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
