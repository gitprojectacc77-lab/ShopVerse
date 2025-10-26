using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopVerse.Data;

namespace ShopVerse.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin/Products")]
    public class AdminProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminProductsController(ApplicationDbContext db) => _db = db;

        // GET: Admin/Products
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var items = await _db.Products.Include(p => p.Category)
                .OrderByDescending(p => p.Id).ToListAsync();
            return View(items);
        }

        // GET: Admin/Products/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View(new Product { IsActive = true });
        }

        // POST: Admin/Products/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid) { await LoadCategoriesAsync(); return View(model); }

            _db.Products.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            await LoadCategoriesAsync(p.CategoryId);
            return View(p);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) { await LoadCategoriesAsync(model.CategoryId); return View(model); }

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Products/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p != null) { _db.Products.Remove(p); await _db.SaveChangesAsync(); }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCategoriesAsync(int? selectedId = null)
        {
            var cats = await _db.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = new SelectList(cats, "Id", "Name", selectedId);
        }
    }
}
