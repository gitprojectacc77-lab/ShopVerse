using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVerse.Data;

namespace ShopVerse.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _users;
        public OrdersController(ApplicationDbContext db, UserManager<IdentityUser> users)
        { _db = db; _users = users; }

        public async Task<IActionResult> My()
        {
            var uid = _users.GetUserId(User)!;
            var orders = await _db.Orders
                .Where(o => o.UserId == uid)
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
            return View(orders);
        }
    }
}
