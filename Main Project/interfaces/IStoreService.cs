using Main_Project.interfaces;
using Main_Project.Models;

namespace Main_Project.interfaces
{
    public interface IStoreService
    {
        Task<List<IShoppingItem>> GetItems();

        IShoppingItem GetItemById(int id);

        Task<bool> Purchase(Purchase purchase);

        string? ValidatePurchase(Purchase purchase);
    }
}