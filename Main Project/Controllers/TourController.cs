using Main_Project.Data;
using Main_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main_Project.Controllers
{
    public class TourController : Controller
    {
        private readonly MainProjectContext _context;

        public TourController(MainProjectContext context)
        {
            _context = context;
        }

        // GET: Tours
        public async Task<IActionResult> Index()
        {
            var tours = await _context.Tours.ToListAsync();
            return View(tours);
        }

        public IActionResult Error()
        {
            return View("Shared/Error");
        }
    }
}