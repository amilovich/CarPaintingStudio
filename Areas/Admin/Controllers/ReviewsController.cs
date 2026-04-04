using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Reviews
        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Service)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            ViewBag.PendingCount  = reviews.Count(r => !r.IsApproved);
            ViewBag.ApprovedCount = reviews.Count(r => r.IsApproved);

            return View(reviews);
        }

        // POST: Admin/Reviews/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            review.IsApproved = true;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Отзивът на {review.AuthorName} е одобрен и публикуван.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Reviews/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            review.IsApproved = false;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Отзивът на {review.AuthorName} е скрит.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return NotFound();

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Отзивът е изтрит успешно.";
            return RedirectToAction(nameof(Index));
        }
    }
}
