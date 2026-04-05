using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Services
{
    public interface IAppointmentService
    {
        Task<PaginatedList<Appointment>> GetAppointmentsAsync(AppointmentFilterViewModel filter, string? userId, bool isAdmin);
        Task<Appointment?> GetByIdAsync(int id);
        Task<Appointment?> GetByIdWithServiceAsync(int id);
        Task<Appointment> CreateAsync(CreateAppointmentViewModel model, string? userId);
        Task<bool> UpdateAsync(int id, Appointment model);
        Task<bool> ChangeStatusAsync(int id, AppointmentStatus status);
        Task<bool> DeleteAsync(int id);
        Task<AppointmentStatsViewModel> GetStatsAsync(string? userId, bool isAdmin);
    }
}
