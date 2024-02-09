using Main_Project.interfaces;
using Main_Project.Data;

using Main_Project.interfaces;

using Microsoft.EntityFrameworkCore;
using Main_Project.Models;
using System.ComponentModel.DataAnnotations;

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

        public async Task<bool> Purchase(Purchase purchase)
        {
            try
            {
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public string? ValidatePurchase(Purchase purchase)
        {
            var validationContext = new ValidationContext(purchase, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(purchase, validationContext, validationResults, validateAllProperties: true);

            if (!isValid && validationResults.Count > 0)
            {
                // Return the first validation error message found.
                return validationResults[0].ErrorMessage;
            }
            return null;
        }
    }
}