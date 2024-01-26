using Main_Project.Models;
using Main_Project.Structs;

namespace Main_Project.interfaces
{
    public interface ICartService
    {
        void AddToCart(int itemId, string itemType, int quantity);

        void RemoveFromCart(int itemId, int quantity, string itemType);

        Dictionary<CartItemKey, int> GetCart();

        int GetCartCount();

        double CalculateTotalPrice(List<IShoppingItem> items);

        void ClearCart();
    }
}