using Main_Project.interfaces;
using Main_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Main_Project.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreService _storeService;
        private readonly ICartService _cartService;

        public StoreController(IStoreService storeService, ICartService cartService)
        {
            _cartService = cartService;
            _storeService = storeService;
        }

        public async Task<IActionResult> Index()
        {
            var cartViewModel = await GetCartViewModelAsync();

            return View(cartViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity, string type)
        {
            _cartService.AddToCart(id, type, quantity);
            var cartViewModel = await GetCartViewModelAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id, int amount, string type)
        {
            try
            {
                _cartService.RemoveFromCart(id, amount, type);
                TempData["SuccessMessage"] = "Item successfully removed from the cart.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            var cartViewModel = await GetCartViewModelAsync();
            return PartialView("_CartItems", cartViewModel);
        }

        private async Task<StoreIndexViewModel> GetCartViewModelAsync()
        {
            var items = await _storeService.GetItems();
            var cart = _cartService.GetCart();

            // Calculate total price for each item in the cart
            var totalPriceSum = _cartService.CalculateTotalPrice(items);

            var data = new StoreIndexViewModel
            {
                StoreItems = items,
                CartItems = cart,
                TotalCartSum = totalPriceSum
            };

            return data;
        }
    }
}