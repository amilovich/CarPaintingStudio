using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews — публично
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
            ViewBag.FiveStars  = reviews.Count(r => r.Rating == 5);
            ViewBag.FourStars  = reviews.Count(r => r.Rating == 4);
            ViewBag.ThreeStars = reviews.Count(r => r.Rating == 3);

            return View(reviews);
        }

        // GET: Reviews/Details/5 — публично
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _context.Reviews
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsApproved);

            if (review == null) return NotFound();

            return View(review);
        }

        // GET: Reviews/Create — само логнати потребители
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new CreateReviewViewModel
            {
                AuthorName = user?.FullName ?? string.Empty,
                AuthorEmail = user?.Email ?? string.Empty,
                AvailableServices = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync()
            };

            return View(model);
        }

        // POST: Reviews/Create — само логнати потребители
        [HttpPost]
        [Authorize]
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
                AuthorName  = model.AuthorName,
                AuthorEmail = model.AuthorEmail,
                Content     = model.Content,
                Rating      = model.Rating,
                ServiceId   = model.ServiceId,
                UserId      = _userManager.GetUserId(User),
                IsApproved  = false,
                CreatedDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Благодарим за вашия отзив! Той ще бъде публикуван след преглед.";
            return RedirectToAction(nameof(Index));
        }
    }
}
