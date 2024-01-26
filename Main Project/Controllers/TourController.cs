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
            return _context.Tours != null ?
                        View(await _context.Tours.ToListAsync()) :
                        Problem("Entity set 'Basic_ProjectContext.Tour'  is null.");
        }

        public IActionResult Error()
        {
            return View("Shared/Error");
        }
    }
}