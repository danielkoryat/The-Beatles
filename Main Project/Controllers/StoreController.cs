using Basic_Project.Data;
using Basic_Project.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basic_Project.Controllers
{
    public class StoreController : Controller
    {
        public StoreController(Basic_ProjectContext context)
        {
            _context = context;
        }

        private readonly Basic_ProjectContext _context;
        private const string CartSessionKey = "Cart";

        public async Task<IActionResult> Index()
        {
            // Execute the first query and await its result before starting the second query
            List<IShoppingItem> albums = await _context.Albums
                .Select(a => (IShoppingItem)a)
                .ToListAsync();

            // Now that the first query has finished, start the second query
            List<IShoppingItem> merchandises = await _context.Merchandises
                .Select(m => (IShoppingItem)m)
                .ToListAsync();

            // Combine the results
            List<IShoppingItem> items = albums.Concat(merchandises).ToList();
            return View(items);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            // Retrieve the cart from session, or create one if it doesn't exist
            var cart = HttpContext.Session.GetObjectFromJson<Dictionary<int, int>>(CartSessionKey) ?? new Dictionary<int, int>();

            |

            // Add to cart or update quantity
            if (cart.ContainsKey(id))
            {
                cart[id] += quantity;
            }
            else
            {
                cart[id] = quantity;
            }

            // Save the updated cart back into the session
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            // Redirect to the desired action or return a view
            return RedirectToAction(nameof(Index));
        }
    }
}