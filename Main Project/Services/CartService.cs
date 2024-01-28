using Main_Project.interfaces;
using Main_Project.Models;
using Main_Project.Structs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;

namespace Main_Project.Services
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyCartItems = "Cart";

        public Dictionary<CartItemKey, int> Items { get; set; } = new();

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Dictionary<CartItemKey, int> GetCart()
        {
            var session = _httpContextAccessor!.HttpContext!.Session;
            var itemsJson = session.GetString(SessionKeyCartItems);
            return string.IsNullOrEmpty(itemsJson) ? new Dictionary<CartItemKey, int>() : DeserializeCart(itemsJson);
        }

        public void AddToCart(int itemId, string itemType, int quantity)
        {
            var key = new CartItemKey(itemId, itemType);

            var items = GetCart();
            if (items.ContainsKey(key))
            {
                items[key] += quantity;
            }
            else
            {
                items.Add(key, quantity);
            }
            SaveCartItems(items);
        }

        public void ClearCart() => SaveCartItems(new Dictionary<CartItemKey, int>());

        public int GetCartCount() => GetCart().Count;

        public double CalculateTotalPrice(List<IShoppingItem> items)
        {
            var cart = GetCart();

            // Convert items to dictionary using composite keys (ItemId, ItemType)
            var itemPriceDictionary = items.ToDictionary(
                item => new CartItemKey(item.Id, item.GetType().Name),
                item => item.Price
                );

            var totalPrice = cart.Sum(cartItem =>
            {
                // Check if the item exists in the itemPriceDictionary
                if (itemPriceDictionary.TryGetValue(cartItem.Key, out var itemPrice))
                {
                    return itemPrice * cartItem.Value;
                }

                return 0;
            });

            return totalPrice;
        }

        public void RemoveFromCart(int itemId, int quantity, string itemType)
        {
            var items = GetCart();
            var key = GenarateKey(itemId, itemType);
            if (items.ContainsKey(key))
            {
                if (quantity <= items[key])
                {
                    items[key] -= quantity;
                    if (items[key] <= 0)
                    {
                        items.Remove(key);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Cannot remove more items than the quantity in the cart.");
                }
            }
            SaveCartItems(items);
        }

        // Helper methods
        private void SaveCartItems(Dictionary<CartItemKey, int> items)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var itemsJson = SerializeCart(items);
            session.SetString(SessionKeyCartItems, itemsJson);
        }

        private CartItemKey GenarateKey(int itemId, string itemType) => new CartItemKey(itemId, itemType);

        public string SerializeCart(Dictionary<CartItemKey, int> cart)
        {
            var jsonObject = new Dictionary<string, int>();
            foreach (var kvp in cart)
            {
                var keyString = $"{kvp.Key.ItemId}-{kvp.Key.ItemType}";
                jsonObject[keyString] = kvp.Value;
            }
            return System.Text.Json.JsonSerializer.Serialize(jsonObject);
        }

        public Dictionary<CartItemKey, int> DeserializeCart(string json)
        {
            var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            var cart = new Dictionary<CartItemKey, int>();

            foreach (var kvp in jsonObject)
            {
                var parts = kvp.Key.Split('-');
                var key = new CartItemKey(int.Parse(parts[0]), parts[1]);
                cart[key] = kvp.Value;
            }
            return cart;
        }
    }
}