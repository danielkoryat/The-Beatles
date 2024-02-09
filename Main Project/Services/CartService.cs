using Main_Project.interfaces;
using Main_Project.Models;
using Main_Project.Structs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;

namespace Main_Project.Services
{
    // Service class for managing shopping cart operations.
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyCartItems = "Cart";

        public Dictionary<CartItemKey, int> Items { get; set; } = new(); // Dictionary to hold cart items.

        // Constructor to inject IHttpContextAccessor for accessing HTTP context.
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieves the current cart from session storage.
        public Dictionary<CartItemKey, int> GetCart()
        {
            var session = _httpContextAccessor!.HttpContext!.Session;
            var itemsJson = session.GetString(SessionKeyCartItems);
            // Deserialize JSON string to cart items dictionary, or return a new dictionary if null.
            return string.IsNullOrEmpty(itemsJson) ? new Dictionary<CartItemKey, int>() : DeserializeCart(itemsJson);
        }

        // Adds a specified quantity of an item to the cart.
        public void AddToCart(int itemId, string itemType, int quantity)
        {
            var key = new CartItemKey(itemId, itemType); // Composite key for cart item.

            var items = GetCart();
            if (items.ContainsKey(key))
            {
                items[key] += quantity; // Increase quantity if item already exists.
            }
            else
            {
                items.Add(key, quantity); // Add new item if it doesn't exist.
            }
            SaveCartItems(items); // Save updated cart items to session.
        }

        // Clears all items from the cart.
        public void ClearCart() => SaveCartItems(new Dictionary<CartItemKey, int>());

        // Returns the count of unique items in the cart.
        public int GetCartCount() => GetCart().Count;

        // Calculates the total price of all items in the cart.
        public double CalculateTotalPrice(List<IShoppingItem> items)
        {
            var cart = GetCart();

            var itemPriceDictionary = items.ToDictionary(
                item => new CartItemKey(item.Id, item.GetType().Name), // Create dictionary with composite keys.
                item => item.Price
                );

            // Calculate total price based on cart item quantities and prices.
            var totalPrice = cart.Sum(cartItem =>
            {
                if (itemPriceDictionary.TryGetValue(cartItem.Key, out var itemPrice))
                {
                    return itemPrice * cartItem.Value; // Multiply item price by quantity.
                }

                return 0; // Return 0 if item is not found in price dictionary.
            });

            return totalPrice;
        }

        // Removes a specified quantity of an item from the cart.
        public void RemoveFromCart(int itemId, int quantity, string itemType)
        {
            var items = GetCart();
            var key = GenarateKey(itemId, itemType); // Generate composite key.
            if (items.ContainsKey(key))
            {
                if (quantity <= items[key])
                {
                    items[key] -= quantity; // Decrease quantity.
                    if (items[key] <= 0)
                    {
                        items.Remove(key); // Remove item if quantity reaches 0 or below.
                    }
                }
                else
                {
                    // Throw exception if trying to remove more items than available.
                    throw new InvalidOperationException("Cannot remove more items than the quantity in the cart.");
                }
            }
            SaveCartItems(items); // Save updated cart items to session.
        }

        // Helper methods
        private void SaveCartItems(Dictionary<CartItemKey, int> items)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var itemsJson = SerializeCart(items); // Serialize cart items to JSON.
            session.SetString(SessionKeyCartItems, itemsJson); // Save JSON string to session.
        }

        private CartItemKey GenarateKey(int itemId, string itemType) => new CartItemKey(itemId, itemType); // Generates a composite key for a cart item.

        // Serializes the cart items dictionary to a JSON string.
        public string SerializeCart(Dictionary<CartItemKey, int> cart)
        {
            var jsonObject = new Dictionary<string, int>();
            foreach (var kvp in cart)
            {
                var keyString = $"{kvp.Key.ItemId}-{kvp.Key.ItemType}"; // Composite key as string.
                jsonObject[keyString] = kvp.Value;
            }
            return System.Text.Json.JsonSerializer.Serialize(jsonObject);
        }

        // Deserializes a JSON string to a cart items dictionary.
        public Dictionary<CartItemKey, int> DeserializeCart(string json)
        {
            var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            var cart = new Dictionary<CartItemKey, int>();

            foreach (var kvp in jsonObject)
            {
                var parts = kvp.Key.Split('-');
                var key = new CartItemKey(int.Parse(parts[0]), parts[1]); // Parse composite key from string.
                cart[key] = kvp.Value;
            }
            return cart;
        }
    }
}