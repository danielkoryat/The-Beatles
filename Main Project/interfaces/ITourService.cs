using Main_Project.Models;

namespace Main_Project.interfaces
{
    public interface ITourService
    {
        void PurchaseTicket(int tourId);

        public List<int> GetPurchasedToursIds();

        Task<List<Tour>> GetAllTours();
    }
}