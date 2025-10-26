using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVerse.Data;

namespace ShopVerse.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CatalogController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? q, int? categoryId)
        {
            var query = _db.Products.Include(p => p.Category).Where(p => p.IsActive);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(p => p.Title.Contains(q));
            if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId);
            ViewBag.Categories = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(await query.OrderByDescending(p => p.Id).ToListAsync());


        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
