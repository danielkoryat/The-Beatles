using Main_Project.interfaces;

namespace Main_Project.interfaces
{
    public interface IStoreService
    {
        Task<List<IShoppingItem>> GetItems();

        IShoppingItem GetItemById(int id);
    }
}