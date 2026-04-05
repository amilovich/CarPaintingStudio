using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public interface IServiceService
    {
        Task<PaginatedList<Service>> GetServicesAsync(ServiceFilterViewModel filter);
        Task<Service?> GetByIdAsync(int id);
        Task<Service?> GetByIdWithReviewsAsync(int id);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
        Task<Service> CreateAsync(ServiceViewModel model);
        Task<bool> UpdateAsync(int id, ServiceViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
