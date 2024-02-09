using Main_Project.Extensions;
using Main_Project.interfaces;
using Main_Project.Models;
using Main_Project.Validations;
using Main_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Main_Project.Controllers
{
    //TODO: verify async methods efficiency

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

            // Render partial view
            string cartItemsPartialView = await this.RenderViewAsync("_CartItems", cartViewModel, partial: true);

            var result = new
            {
                success = true,
                html = cartItemsPartialView,
                message = "Item has been added to your cart."
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id, int amount, string type)
        {
            _cartService.RemoveFromCart(id, amount, type);
            var cartViewModel = await GetCartViewModelAsync();
            string cartItemsPartialView = await this.RenderViewAsync("_CartItems", cartViewModel, partial: true);

            var result = new
            {
                success = true,
                html = cartItemsPartialView,
                message = "Item has been removed from your cart."
            };

            return Json(result);
        }

        public async Task<IActionResult> Purchase(string fullName)
        {
            var items = await _storeService.GetItems();
            double cartTotalPrice = _cartService.CalculateTotalPrice(items);

            var purchase = new Purchase { FullName = fullName, Amount = cartTotalPrice };
            string errorMessage = _storeService.ValidatePurchase(purchase);
            if (errorMessage != null)
            {
                return this.CreateResponse(message: errorMessage, success: false);
            }
            bool isSucsess = await _storeService.Purchase(purchase);
            if (!isSucsess)
            {
                return this.CreateResponse(message: "Purchase failed", success: false);
            }

            _cartService.ClearCart();
            var storeIndexViewModel = await GetCartViewModelAsync();
            var partialView = await this.RenderViewAsync("_CartItems", storeIndexViewModel, partial: true);

            return this.CreateResponse(message: "Thank you for your purchase!", html: partialView);
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