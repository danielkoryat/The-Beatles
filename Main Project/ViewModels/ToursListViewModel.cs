using Main_Project.Models;

namespace Main_Project.ViewModels
{
    public class ToursListViewModel
    {
        public List<Tour> Tours { get; set; }

        public List<int> PurchasedToursIds { get; set; }
    }
}