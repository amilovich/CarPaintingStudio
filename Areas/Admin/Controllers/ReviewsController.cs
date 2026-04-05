using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarPaintingStudio.Services;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: Admin/Reviews
        public async Task<IActionResult> Index()
        {
            var stats = await _reviewService.GetStatsAsync();
            ViewBag.PendingCount  = stats.PendingCount;
            ViewBag.ApprovedCount = stats.TotalApproved;

            // За таблицата вземаме всички (одобрени + неодобрени)
            var approved   = await _reviewService.GetApprovedReviewsAsync();
            // Комбинираме — използваме отделен метод ако искаме всички
            // За сега показваме само одобрените + pending се вижда от stats
            return View(approved);
        }

        // POST: Admin/Reviews/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var review  = await _reviewService.GetByIdAsync(id);
            var result  = await _reviewService.ApproveAsync(id);
            if (!result) return NotFound();

            TempData["SuccessMessage"] = $"Отзивът на {review?.AuthorName} е одобрен и публикуван.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Reviews/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            var result = await _reviewService.RejectAsync(id);
            if (!result) return NotFound();

            TempData["SuccessMessage"] = $"Отзивът на {review?.AuthorName} е скрит.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewService.DeleteAsync(id);
            if (!result) return NotFound();

            TempData["SuccessMessage"] = "Отзивът е изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}
