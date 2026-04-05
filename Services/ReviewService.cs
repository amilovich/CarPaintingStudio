using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetApprovedReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Service)
                .Where(r => r.IsApproved)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> CreateAsync(CreateReviewViewModel model, string? userId)
        {
            var review = new Review
            {
                AuthorName  = model.AuthorName,
                AuthorEmail = model.AuthorEmail,
                Content     = model.Content,
                Rating      = model.Rating,
                ServiceId   = model.ServiceId,
                UserId      = userId,
                IsApproved  = false,
                CreatedDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> ApproveAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            review.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            review.IsApproved = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReviewStatsViewModel> GetStatsAsync()
        {
            var approved = await _context.Reviews
                .Where(r => r.IsApproved)
                .ToListAsync();

            return new ReviewStatsViewModel
            {
                TotalApproved = approved.Count,
                PendingCount  = await _context.Reviews.CountAsync(r => !r.IsApproved),
                AverageRating = approved.Any() ? Math.Round(approved.Average(r => r.Rating), 1) : 0,
                FiveStars     = approved.Count(r => r.Rating == 5),
                FourStars     = approved.Count(r => r.Rating == 4),
                ThreeStars    = approved.Count(r => r.Rating == 3),
                TwoStars      = approved.Count(r => r.Rating == 2),
                OneStar       = approved.Count(r => r.Rating == 1)
            };
        }
    }
}
