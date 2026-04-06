using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public interface IReviewService
    {
        Task<PaginatedList<Review>> GetApprovedReviewsAsync(ReviewFilterViewModel filter);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> CreateAsync(CreateReviewViewModel model, string? userId);
        Task<bool> ApproveAsync(int id);
        Task<bool> RejectAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<ReviewStatsViewModel> GetStatsAsync();
    }
}
