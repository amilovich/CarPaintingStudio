using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Service)
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            ViewBag.AverageRating = reviews.Any()
                ? Math.Round(reviews.Average(r => r.Rating), 1)
                : 0;

            ViewBag.TotalReviews = reviews.Count;
            ViewBag.FiveStars = reviews.Count(r => r.Rating == 5);
            ViewBag.FourStars = reviews.Count(r => r.Rating == 4);
            ViewBag.ThreeStars = reviews.Count(r => r.Rating == 3);

            return View(reviews);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsApproved);

            if (review == null) return NotFound();

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create()
        {
            var model = new CreateReviewViewModel
            {
                AvailableServices = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync()
            };

            return View(model);
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableServices = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                return View(model);
            }

            var review = new Review
            {
                AuthorName = model.AuthorName,
                AuthorEmail = model.AuthorEmail,
                Content = model.Content,
                Rating = model.Rating,
                ServiceId = model.ServiceId,
                IsApproved = false, // Изисква одобрение от Admin
                CreatedDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Благодарим за вашия отзив! Той ще бъде публикуван след преглед.";
            return RedirectToAction(nameof(Index));
        }
    }
}
