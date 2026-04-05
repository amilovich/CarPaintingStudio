using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;
        private const int DefaultPageSize = 8;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Appointment>> GetAppointmentsAsync(
            AppointmentFilterViewModel filter, string? userId, bool isAdmin)
        {
            var query = _context.Appointments
                .Include(a => a.Service)
                .AsQueryable();

            // Не-Admin виждат само своите записвания
            if (!isAdmin && userId != null)
                query = query.Where(a => a.UserId == userId);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.ToLower();
                query = query.Where(a =>
                    a.CustomerName.ToLower().Contains(s) ||
                    a.CarBrand.ToLower().Contains(s) ||
                    a.CarModel.ToLower().Contains(s) ||
                    a.Email.ToLower().Contains(s));
            }

            if (!string.IsNullOrEmpty(filter.Status) &&
                Enum.TryParse<AppointmentStatus>(filter.Status, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            if (filter.DateFrom.HasValue)
                query = query.Where(a => a.AppointmentDate >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                query = query.Where(a => a.AppointmentDate <= filter.DateTo.Value);

            query = query.OrderByDescending(a => a.AppointmentDate);

            return await PaginatedList<Appointment>.CreateAsync(
                query, filter.Page < 1 ? 1 : filter.Page, DefaultPageSize);
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<Appointment?> GetByIdWithServiceAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> CreateAsync(CreateAppointmentViewModel model, string? userId)
        {
            var appointment = new Appointment
            {
                CustomerName    = model.CustomerName,
                Phone           = model.Phone,
                Email           = model.Email,
                CarBrand        = model.CarBrand,
                CarModel        = model.CarModel,
                CarYear         = model.CarYear,
                AppointmentDate = model.AppointmentDate,
                Notes           = model.Notes,
                ServiceId       = model.ServiceId,
                UserId          = userId,
                Status          = AppointmentStatus.Pending,
                CreatedDate     = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> UpdateAsync(int id, Appointment model)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            appointment.CustomerName    = model.CustomerName;
            appointment.Phone           = model.Phone;
            appointment.Email           = model.Email;
            appointment.CarBrand        = model.CarBrand;
            appointment.CarModel        = model.CarModel;
            appointment.CarYear         = model.CarYear;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Notes           = model.Notes;
            appointment.ServiceId       = model.ServiceId;
            appointment.Status          = model.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            appointment.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<AppointmentStatsViewModel> GetStatsAsync(string? userId, bool isAdmin)
        {
            var query = _context.Appointments.AsQueryable();

            if (!isAdmin && userId != null)
                query = query.Where(a => a.UserId == userId);

            return new AppointmentStatsViewModel
            {
                PendingCount    = await query.CountAsync(a => a.Status == AppointmentStatus.Pending),
                ConfirmedCount  = await query.CountAsync(a => a.Status == AppointmentStatus.Confirmed),
                InProgressCount = await query.CountAsync(a => a.Status == AppointmentStatus.InProgress),
                CompletedCount  = await query.CountAsync(a => a.Status == AppointmentStatus.Completed),
                CancelledCount  = await query.CountAsync(a => a.Status == AppointmentStatus.Cancelled)
            };
        }
    }
}
