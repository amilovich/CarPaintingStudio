using Microsoft.AspNetCore.Mvc;
using CarPaintingStudio.Data;
using System.Diagnostics;
using CarPaintingStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace CarPaintingStudio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .Take(3)
                .ToListAsync();

            var galleryItems = await _context.GalleryItems
                .Where(g => g.IsVisible)
                .OrderByDescending(g => g.CompletedDate)
                .Take(4)
                .ToListAsync();

            ViewBag.Services = services;
            ViewBag.GalleryItems = galleryItems;

            return View();
        }

        public async Task<IActionResult> About()
        {
            var employees = await _context.Employees
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.YearsOfExperience)
                .ToListAsync();

            return View(employees);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
