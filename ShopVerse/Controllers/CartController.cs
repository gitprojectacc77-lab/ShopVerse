using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVerse.Data;
using ShopVerse.Models;
using ShopVerse.Services;

namespace ShopVerse.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cart;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _users;
        public CartController(ICartService cart, ApplicationDbContext db, UserManager<IdentityUser> users)
        { _cart = cart; _db = db; _users = users; }

        [HttpGet] public IActionResult Index() => View(_cart.Get());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id, int qty = 1)
        { await _cart.AddAsync(id, qty); return RedirectToAction("Index"); }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(int id, int qty)
        { _cart.UpdateQty(id, qty); return RedirectToAction("Index"); }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        { _cart.Remove(id); return RedirectToAction("Index"); }

        [Authorize, HttpGet]
        public IActionResult Checkout()
        { var cart = _cart.Get(); if (cart.IsEmpty) return RedirectToAction("Index"); return View(cart); }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckoutConfirm()
        {
            var cart = _cart.Get(); if (cart.IsEmpty) return RedirectToAction("Index");
            var order = new Order { UserId = _users.GetUserId(User)!, Status = "New", Total = cart.Total, Items = new List<OrderItem>() };

            var ids = cart.Items.Select(i => i.ProductId).ToList();
            var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
            foreach (var line in cart.Items)
                order.Items.Add(new OrderItem { ProductId = line.ProductId, Quantity = line.Quantity, UnitPrice = line.UnitPrice });

            _db.Orders.Add(order); await _db.SaveChangesAsync(); _cart.Clear();
            return RedirectToAction("Success", new { id = order.Id });
        }

        [Authorize, HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _db.Orders.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id);
            return order == null ? NotFound() : View(order);
        }
    }
}
