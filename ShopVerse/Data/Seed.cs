using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ShopVerse.Data.ApplicationDbContext;

namespace ShopVerse.Data
{
    public static class Seed
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await ctx.Database.MigrateAsync();

            if (!await roleMgr.RoleExistsAsync("Admin"))
                await roleMgr.CreateAsync(new IdentityRole("Admin"));

            var adminEmail = "admin@shopverse.local";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userMgr.CreateAsync(admin, "Admin123!");
                await userMgr.AddToRoleAsync(admin, "Admin");
            }

            if (!await ctx.Categories.AnyAsync())
            {
                var electronics = new Category { Name = "Electronics" };
                var books = new Category { Name = "Books" };
                ctx.Categories.AddRange(electronics, books);
                await ctx.SaveChangesAsync();

                ctx.Products.AddRange(
                    new Product { Title = "Headphones", Price = 59.99m, Category = electronics, ImageUrl = "https://picsum.photos/seed/101/600/400" },
                    new Product { Title = "Smartwatch", Price = 129.99m, Category = electronics, ImageUrl = "https://picsum.photos/seed/102/600/400" },
                    new Product { Title = "C# in Depth", Price = 39.99m, Category = books, ImageUrl = "https://picsum.photos/seed/103/600/400" }
                );
                await ctx.SaveChangesAsync();
            }
        }
    }
}
