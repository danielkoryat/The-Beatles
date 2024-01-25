using Main_Project.interfaces;
using Main_Project.Data;

using Main_Project.interfaces;

using Microsoft.EntityFrameworkCore;

namespace Main_Project.Services
{
    public class StoreService : IStoreService
    {
        private readonly MainProjectContext _context;

        public StoreService(MainProjectContext context)
        {
            _context = context;
        }

        public IShoppingItem? GetItemById(int id)
        {
            IShoppingItem? item = _context.Albums.FirstOrDefault(a => a.Id == id);
            item ??= _context.Merchandises.FirstOrDefault(m => m.Id == id);
            return item;
        }

        public async Task<List<IShoppingItem>> GetItems()
        {
            List<IShoppingItem> albums = await _context.Albums
                .Select(a => (IShoppingItem)a)
                .ToListAsync();

            List<IShoppingItem> merchandises = await _context.Merchandises
                .Select(m => (IShoppingItem)m)
                .ToListAsync();

            // Combine the results
            List<IShoppingItem> items = albums.Concat(merchandises).ToList();

            return items;
        }
    }
}