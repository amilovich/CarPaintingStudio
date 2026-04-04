using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;

namespace CarPaintingStudio.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gallery — публично
        public async Task<IActionResult> Index()
        {
            var galleryItems = await _context.GalleryItems
                .Where(g => g.IsVisible)
                .OrderByDescending(g => g.CompletedDate)
                .ToListAsync();

            return View(galleryItems);
        }
    }
}
