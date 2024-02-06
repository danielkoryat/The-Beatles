using Main_Project.Data;
using Main_Project.interfaces;
using Main_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Main_Project.Services
{
    // Service class for managing tour-related operations.
    public class TourService : ITourService
    {
        private readonly MainProjectContext _context; // Database context for entity framework operations.
        private readonly IHttpContextAccessor _httpContextAccessor; // Accessor to manage HTTP context and sessions.
        private const string SessionKeyTourPurchase = "Tour"; // Session key for storing purchased tour IDs.

        // Constructor to initialize context and http context accessor.
        public TourService(MainProjectContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Purchases a ticket for a specified tour by adding its ID to the session.
        public void PurchaseTicket(int tourId)
        {
            // Retrieve the tour from the database.
            var tour = _context.Tours.FirstOrDefault(t => t.Id == tourId);

            // Throw an exception if the tour does not exist.
            if (tour == null)
                throw new Exception("Tour does not exist");

            var tourIds = GetPurchasedToursIds(); // Get the list of already purchased tour IDs.
            if (!tourIds.Contains(tourId))
            {
                tourIds.Add(tourId); // Add the new tour ID if not already purchased.
                SaveToursIds(tourIds); // Save the updated list of purchased tour IDs to the session.
            }
        }

        // Retrieves all tours from the database asynchronously.
        public async Task<List<Tour>> GetAllTours() => await _context.Tours.ToListAsync();

        // Gets a list of purchased tour IDs from the session.
        public List<int> GetPurchasedToursIds()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var value = session.GetString(SessionKeyTourPurchase);
            // Deserialize the session value to a list of integers or return a new list if null.
            return value == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(value);
        }

        // Saves the list of purchased tour IDs to the session.
        private void SaveToursIds(List<int> tourIds)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var stringedIds = JsonConvert.SerializeObject(tourIds); // Serialize the list to a JSON string.
            session.SetString(SessionKeyTourPurchase, stringedIds); // Store the serialized string in the session.
        }
    }
}