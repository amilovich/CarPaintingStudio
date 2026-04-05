using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;
        private const int DefaultPageSize = 6;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Service>> GetServicesAsync(ServiceFilterViewModel filter)
        {
            var query = _context.Services
                .Where(s => s.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(s) ||
                    x.Description.ToLower().Contains(s));
            }

            if (filter.MinPrice.HasValue)
                query = query.Where(x => x.Price >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(x => x.Price <= filter.MaxPrice.Value);

            query = filter.SortBy switch
            {
                "price_asc"     => query.OrderBy(x => x.Price),
                "price_desc"    => query.OrderByDescending(x => x.Price),
                "duration_asc"  => query.OrderBy(x => x.DurationDays),
                "duration_desc" => query.OrderByDescending(x => x.DurationDays),
                "name_asc"      => query.OrderBy(x => x.Name),
                _               => query.OrderByDescending(x => x.CreatedDate)
            };

            return await PaginatedList<Service>.CreateAsync(
                query, filter.Page < 1 ? 1 : filter.Page, DefaultPageSize);
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<Service?> GetByIdWithReviewsAsync(int id)
        {
            return await _context.Services
                .Include(s => s.Reviews.Where(r => r.IsApproved))
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
            return await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Service> CreateAsync(ServiceViewModel model)
        {
            var service = new Service
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DurationDays = model.DurationDays,
                IsActive = model.IsActive,
                CreatedDate = DateTime.Now
            };

            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> UpdateAsync(int id, ServiceViewModel model)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            service.Name = model.Name;
            service.Description = model.Description;
            service.Price = model.Price;
            service.DurationDays = model.DurationDays;
            service.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return false;
            if (service.Appointments.Any()) return false;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return false;

            service.IsActive = !service.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Services.AnyAsync(s => s.Id == id);
        }
    }
}
