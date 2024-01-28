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

        public TourController(MainProjectContext context, ITourService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var viewModel = await GetToursListViewModeAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> PurchaseTicket(int tourId)
        {
            _tourService.PurchaseTicket(tourId);

            var toursListViewModel = await GetToursListViewModeAsync();

            var toursListPartialView = await this.RenderViewAsync("_ToursList", toursListViewModel, partial: true);

            var result = new ResultObject
            {
                Success = true,
                Html = toursListPartialView,
                Message = "Ticket has been purchased"
            };

            return Json(result);
        }

        public IActionResult Error()
        {
            return View("Shared/Error");
        }

        private async Task<ToursListViewModel> GetToursListViewModeAsync()
        {
            var tours = await _tourService.GetAllTours();
            var purchasedToursIds = _tourService.GetPurchasedToursIds();

            var viewModel = new ToursListViewModel { PurchasedToursIds = purchasedToursIds, Tours = tours };

            return viewModel;
        }
    }
}