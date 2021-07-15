using BlogMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Table
            modelBuilder.Entity<BlogPost>().ToTable("BlogPosts");
            modelBuilder.Entity<Category>().ToTable("Categories");

            //Primary Keys
            modelBuilder.Entity<BlogPost>().HasKey(bp => bp.Id).HasName("PK_BlogPosts");
            modelBuilder.Entity<Category>().HasKey(c => c.Id).HasName("PK_Categories");
            //Indexes implemented later

            //Columns
            modelBuilder.Entity<BlogPost>().Property(bp => bp.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<BlogPost>().Property(bp => bp.Title).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<BlogPost>().Property(bp => bp.Description).HasColumnType("nvarchar(250)").IsRequired(false);
            modelBuilder.Entity<BlogPost>().Property(bp => bp.HomePageContent).IsRequired();
            modelBuilder.Entity<BlogPost>().Property(bp => bp.Content).IsRequired();
            modelBuilder.Entity<BlogPost>().Property(bp => bp.PostDate).HasColumnType("datetime").IsRequired();

            modelBuilder.Entity<Category>().Property(c => c.Id).HasColumnType("int").UseMySqlIdentityColumn().IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Name).HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description).HasColumnType("nvarchar(250)").IsRequired(false);
            modelBuilder.Entity<Category>().Property(c => c.Image).IsRequired(false);


            //Configure foreign key releationship later
            modelBuilder.Entity<BlogPost>()
                .HasOne(bp => bp.Category)
                .WithMany(c=>c.BlogPosts)
                .HasForeignKey(bp=>bp.CategoryId);

        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
