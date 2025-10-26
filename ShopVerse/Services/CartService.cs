using ShopVerse.Data;
using ShopVerse.Models;
using ShopVerse.Utils;

namespace ShopVerse.Services
{
    public interface ICartService
    {
        CartVm Get();
        Task AddAsync(int productId, int qty = 1);
        void UpdateQty(int productId, int qty);
        void Remove(int productId);
        void Clear();
    }

    public class CartService : ICartService
    {
        private const string CartKey = "CART_V1";
        private readonly IHttpContextAccessor _http;
        private readonly ApplicationDbContext _db;

        public CartService(IHttpContextAccessor http, ApplicationDbContext db)
        {
            _http = http;
            _db = db;
        }

        private ISession Session => _http.HttpContext!.Session;

        public CartVm Get()
            => Session.GetObject<CartVm>(CartKey) ?? new CartVm();

        private void Save(CartVm vm) => Session.SetObject(CartKey, vm);

        public async Task AddAsync(int productId, int qty = 1)
        {
            var cart = Get();
            var line = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (line == null)
            {
                var p = await _db.Products.FindAsync(productId)
                        ?? throw new InvalidOperationException("Product not found");
                cart.Items.Add(new CartItemVm
                {
                    ProductId = p.Id,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    UnitPrice = p.Price,
                    Quantity = qty
                });
            }
            else
            {
                line.Quantity += qty;
            }
            Save(cart);
        }

        public void UpdateQty(int productId, int qty)
        {
            var cart = Get();
            var line = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (line != null)
            {
                line.Quantity = Math.Max(1, qty);
                Save(cart);
            }
        }

        public void Remove(int productId)
        {
            var cart = Get();
            cart.Items.RemoveAll(i => i.ProductId == productId);
            Save(cart);
        }

        public void Clear() => Save(new CartVm());
    }
}
