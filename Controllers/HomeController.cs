using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
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

        // Custom 404 - Not Found
        [Route("Home/NotFound")]
        [Route("404")]
        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // Custom 500 - Server Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error,
                    "Unhandled exception on path {Path}", exceptionFeature.Path);
            }

            ViewBag.RequestId = requestId;
            ViewBag.ShowRequestId = !string.IsNullOrEmpty(requestId);

            return View();
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
