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
            ViewBag.TotalServices = await _context.Services.CountAsync();
            ViewBag.ActiveServices = await _context.Services.CountAsync(s => s.IsActive);
            ViewBag.TotalAppointments = await _context.Appointments.CountAsync();
            ViewBag.PendingAppointments = await _context.Appointments
                .CountAsync(a => a.Status == AppointmentStatus.Pending);
            ViewBag.TotalEmployees = await _context.Employees.CountAsync(e => e.IsActive);
            ViewBag.TotalGalleryItems = await _context.GalleryItems.CountAsync(g => g.IsVisible);

            var recentAppointments = await _context.Appointments
                .Include(a => a.Service)
                .OrderByDescending(a => a.CreatedDate)
                .Take(5)
                .ToListAsync();

            return View(recentAppointments);
        }
    }
}
