using BlogMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data
{
    public interface IRepository
    {
        Task AddCategoryAsync(Category category);
        Task EditCategoryAsync(Category category);
        Task<Category> FindCategoryAsync(int? id);
        Task DeleteCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoriesAsync();

        Task AddBlogPostAsync(BlogPost blogPost);
        Task EditBlogPostAsync(BlogPost blogPost);
        Task<BlogPost> FindBlogPostAsync(int? id);
        Task DeleteBlogPostAsync(BlogPost blogPost);
        Task<List<BlogPost>> GetAllBlogPostsWithCategoryAsync();
        Task<List<BlogPost>> GetAllBlogPostsAsync();
        Task<List<BlogPost>> GetAllBlogPostsByCategoryAsync(int categoryId);
    }
}
