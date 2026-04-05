using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalServices       = await _context.Services.CountAsync();
            ViewBag.ActiveServices      = await _context.Services.CountAsync(s => s.IsActive);
            ViewBag.TotalAppointments   = await _context.Appointments.CountAsync();
            ViewBag.PendingAppointments = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
            ViewBag.TotalEmployees      = await _context.Employees.CountAsync(e => e.IsActive);
            ViewBag.TotalGalleryItems   = await _context.GalleryItems.CountAsync(g => g.IsVisible);
            ViewBag.TotalReviews        = await _context.Reviews.CountAsync();
            ViewBag.PendingReviews      = await _context.Reviews.CountAsync(r => !r.IsApproved);
            ViewBag.TotalUsers          = await _context.Users.CountAsync();

            // Приходи от завършени записвания
            var completedRevenue = await _context.Appointments
                .Include(a => a.Service)
                .Where(a => a.Status == AppointmentStatus.Completed)
                .SumAsync(a => (decimal?)a.Service!.Price) ?? 0;
            ViewBag.CompletedRevenue = completedRevenue;

            var recentAppointments = await _context.Appointments
                .Include(a => a.Service)
                .OrderByDescending(a => a.CreatedDate)
                .Take(5)
                .ToListAsync();

            return View(recentAppointments);
        }

        // AJAX endpoint за статистика
        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                totalAppointments   = await _context.Appointments.CountAsync(),
                pendingCount        = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending),
                confirmedCount      = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Confirmed),
                inProgressCount     = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.InProgress),
                completedCount      = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Completed),
                cancelledCount      = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Cancelled),
                pendingReviews      = await _context.Reviews.CountAsync(r => !r.IsApproved),
                totalUsers          = await _context.Users.CountAsync()
            };

            return Json(stats);
        }
    }
}
