using Main_Project.Data;
using Main_Project.Extensions;
using Main_Project.interfaces;
using Main_Project.Models;
using Main_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main_Project.Controllers
{
    // Controller class for handling tour-related requests.
    public class TourController : Controller
    {
        private readonly MainProjectContext _context; // Database context for entity framework operations.
        private readonly ITourService _tourService; // Service layer for business logic related to tours.

        // Constructor to initialize the database context and tour service.
        public TourController(MainProjectContext context, ITourService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

        // Action method for the index page, displays a list of tours.
        public async Task<IActionResult> Index()
        {
            var viewModel = await GetToursListViewModeAsync(); // Fetches the tours and purchased tours IDs.
            return View(viewModel); // Renders the view with the provided ViewModel.
        }

        // Action method to handle ticket purchase requests.
        public async Task<IActionResult> PurchaseTicket(int tourId)
        {
            _tourService.PurchaseTicket(tourId); // Calls the service to purchase a ticket for the given tour ID.

            var toursListViewModel = await GetToursListViewModeAsync(); // Updates the list of tours after purchase.

            // Renders the tours list partial view as a string.
            var toursListPartialView = await this.RenderViewAsync("_ToursList", toursListViewModel, partial: true);

            // Prepares a JSON response with the result of the ticket purchase.
            var result = new ResultObject
            {
                Success = true,
                Html = toursListPartialView,
                Message = "Ticket has been purchased"
            };

            return Json(result); // Returns the result as JSON to the client.
        }

        // Action method for handling errors.
        public IActionResult Error()
        {
            return View("Shared/Error"); // Renders a shared error view.
        }

        // Helper method to construct the ViewModel for tours list view.
        private async Task<ToursListViewModel> GetToursListViewModeAsync()
        {
            var tours = await _tourService.GetAllTours(); // Retrieves all tours.
            var purchasedToursIds = _tourService.GetPurchasedToursIds(); // Gets the list of purchased tour IDs.

            // Prepares the ViewModel with tours and purchased tours information.
            var viewModel = new ToursListViewModel { PurchasedToursIds = purchasedToursIds, Tours = tours };

            return viewModel; // Returns the constructed ViewModel.
        }
    }
}