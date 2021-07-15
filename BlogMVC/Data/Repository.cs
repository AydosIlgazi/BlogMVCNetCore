using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _db;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }
        public async Task EditCategoryAsync(Category category)
        {

            _db.Categories.Update(category);
            await _db.SaveChangesAsync();

        }

        public async Task<Category> FindCategoryAsync(int? id)
        {
            var category = await _db.Categories.FindAsync(id);
            return category;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            return categories;
        }
        public async Task DeleteCategoryAsync(Category category)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task AddBlogPostAsync(BlogPost blogPost)
        {
            await _db.BlogPosts.AddAsync(blogPost);
            await _db.SaveChangesAsync();
        }

        public async Task EditBlogPostAsync(BlogPost blogPost)
        {
            _db.BlogPosts.Update(blogPost);
            await _db.SaveChangesAsync();
        }

        public async Task<BlogPost> FindBlogPostAsync(int? id)
        {
            var blogPost = await _db.BlogPosts.FindAsync(id);
            return blogPost;
        }

        public async Task DeleteBlogPostAsync(BlogPost blogPost)
        {
            _db.BlogPosts.Remove(blogPost);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BlogPost>> GetAllBlogPostsWithCategoryAsync()
        {
            var blogPosts = await _db.BlogPosts.Include(bp => bp.Category)
                .OrderByDescending(bp => bp.PostDate).ToListAsync();
            return blogPosts;
        }
        public async Task<List<BlogPost>> GetAllBlogPostsByCategoryAsync(int categoryId)
        {
            var blogPosts = await _db.BlogPosts.Include(bp => bp.Category)
                .Where(bp => bp.CategoryId == categoryId)
                .OrderByDescending(bp => bp.PostDate).ToListAsync();
            return blogPosts;
        }

        public async Task<List<BlogPost>> GetAllBlogPostsAsync()
        {
            var blogPosts = await _db.BlogPosts
                .OrderByDescending(bp => bp.PostDate).ToListAsync();
            return blogPosts;
        }
    }
}
