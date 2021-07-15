using BlogMVC.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                var scope = host.Services.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                dbContext.Database.EnsureCreated();


                var contentManagerRole = new IdentityRole("contentmanager");
                if (!dbContext.Roles.Any())
                {
                    //create role

                    roleManager.CreateAsync(contentManagerRole).GetAwaiter().GetResult();
                }

                if (!dbContext.Users.Any(u => u.UserName == "admin"))
                {
                    //create an user as content manager

                    var contentManagerUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com",
                    };
                    var result = userManager.CreateAsync(contentManagerUser, "password").GetAwaiter().GetResult();

                    //add role user

                    userManager.AddToRoleAsync(contentManagerUser, contentManagerRole.Name).GetAwaiter().GetResult();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>();
    }
}
