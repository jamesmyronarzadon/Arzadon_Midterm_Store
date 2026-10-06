using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arzadon_Midterm_Store.Data;
using Arzadon_Midterm_Store.Models;

namespace Arzadon_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ - Show cart
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.ToListAsync();

            return View(cartItems);
        }

        // ADD TO CART
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }

        // UPDATE QUANTITY
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            cartItem.Quantity = quantity;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // REMOVE FROM CART
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}