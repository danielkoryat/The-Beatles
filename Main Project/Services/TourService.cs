using Main_Project.Data;
using Main_Project.interfaces;
using Main_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Main_Project.Services
{
    public class TourService : ITourService
    {
        private readonly MainProjectContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyTourPurchase = "Tour";

        public TourService(MainProjectContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void PurchaseTicket(int tourId)
        {
            var tour = _context.Tours.FirstOrDefault(t => t.Id == tourId);

            //TODO: implament error handling
            if (tour == null)
                throw new Exception("Tour does not exist");

            var tourIds = GetPurchasedToursIds();
            if (!tourIds.Contains(tourId))
            {
                tourIds.Add(tourId);
                SaveToursIds(tourIds);
            }
        }

        public async Task<List<Tour>> GetAllTours() => await _context.Tours.ToListAsync();

        public List<int> GetPurchasedToursIds()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var value = session.GetString(SessionKeyTourPurchase);
            return value == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(value);
        }

        private void SaveToursIds(List<int> tourIds)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var stringedIds = JsonConvert.SerializeObject(tourIds);
            session.SetString(SessionKeyTourPurchase, stringedIds);
        }
    }
}