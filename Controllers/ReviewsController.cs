using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IServiceService _serviceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            IReviewService reviewService,
            IServiceService serviceService,
            UserManager<ApplicationUser> userManager)
        {
            _reviewService  = reviewService;
            _serviceService = serviceService;
            _userManager    = userManager;
        }

        // GET: Reviews — публично с pagination и search/filter
        public async Task<IActionResult> Index(ReviewFilterViewModel filter)
        {
            var reviews = await _reviewService.GetApprovedReviewsAsync(filter);
            var stats   = await _reviewService.GetStatsAsync();

            ViewBag.AverageRating = stats.AverageRating;
            ViewBag.TotalReviews  = stats.TotalApproved;
            ViewBag.FiveStars     = stats.FiveStars;
            ViewBag.FourStars     = stats.FourStars;
            ViewBag.ThreeStars    = stats.ThreeStars;
            ViewBag.Filter        = filter;

            return View(reviews);
        }

        // GET: Reviews/Details/5 — публично
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var review = await _reviewService.GetByIdAsync(id.Value);
            if (review == null || !review.IsApproved) return NotFound();

            return View(review);
        }

        // GET: Reviews/Create — само логнати
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user     = await _userManager.GetUserAsync(User);
            var services = await _serviceService.GetActiveServicesAsync();

            var model = new CreateReviewViewModel
            {
                AuthorName        = user?.FullName ?? string.Empty,
                AuthorEmail       = user?.Email    ?? string.Empty,
                AvailableServices = services.ToList()
            };

            return View(model);
        }

        // POST: Reviews/Create — само логнати
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var services = await _serviceService.GetActiveServicesAsync();
                model.AvailableServices = services.ToList();
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            await _reviewService.CreateAsync(model, userId);

            TempData["SuccessMessage"] = "Благодарим за вашия отзив! Той ще бъде публикуван след преглед.";
            return RedirectToAction(nameof(Index));
        }
    }
}
