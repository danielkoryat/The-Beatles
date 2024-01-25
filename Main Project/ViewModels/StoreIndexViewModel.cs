using Main_Project.interfaces;
using Main_Project.Structs;

namespace Main_Project.ViewModels
{
    public class StoreIndexViewModel
    {
        public IEnumerable<IShoppingItem> StoreItems { get; set; }

        public Dictionary<CartItemKey, int> CartItems { get; set; }

        public double TotalCartSum { get; set; }
    }
}