using Main_Project.Data;
using Main_Project.Extensions;
using Main_Project.interfaces;
using Main_Project.Models;
using Main_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main_Project.Controllers
{
    public class TourController : Controller
    {
        private readonly MainProjectContext _context;
        private readonly ITourService _tourService;

        // Constructor to initialize the database context and tour service.
        public TourController(MainProjectContext context, ITourService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

        // Action method for the index page, displays a list of tours.
        public async Task<IActionResult> Index()
        {
            var viewModel = await GetToursListViewModeAsync();
            return View(viewModel);
        }

        // Action method to handle ticket purchase requests.
        public async Task<IActionResult> PurchaseTicket(int tourId)
        {
            _tourService.PurchaseTicket(tourId);

            var toursListViewModel = await GetToursListViewModeAsync();

            // Renders the tours list partial view as a string.
            var toursListPartialView = await this.RenderViewAsync("_ToursList", toursListViewModel, partial: true);

            return this.CreateResponse(message: "Ticket has been purchased", html: toursListPartialView, success: true);
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

            return viewModel;
        }
    }
}