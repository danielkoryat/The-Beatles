﻿using Main_Project.interfaces;
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
        private const string SessionKeyCartItems = "advancedProgrammer99";

        public Dictionary<CartItemKey, int> Items { get; set; } = new();

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private void SaveCartItems(Dictionary<CartItemKey, int> items)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var itemsJson = SerializeCart(items);
            session.SetString(SessionKeyCartItems, itemsJson);
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

        public decimal GetCartTotal() => GetCart().Sum(x => x.Value);

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