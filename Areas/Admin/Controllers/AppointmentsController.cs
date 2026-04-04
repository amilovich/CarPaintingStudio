using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Appointments
        public async Task<IActionResult> Index(string? status, string? search)
        {
            var query = _context.Appointments
                .Include(a => a.Service)
                .AsQueryable();

            // Филтриране по статус
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<AppointmentStatus>(status, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            // Търсене по клиент или марка
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(a =>
                    a.CustomerName.ToLower().Contains(search) ||
                    a.CarBrand.ToLower().Contains(search) ||
                    a.CarModel.ToLower().Contains(search) ||
                    a.Email.ToLower().Contains(search));
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            ViewBag.CurrentSearch = search;
            ViewBag.TotalCount = await _context.Appointments.CountAsync();
            ViewBag.PendingCount = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Pending);
            ViewBag.ConfirmedCount = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Confirmed);
            ViewBag.CompletedCount = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Completed);
            ViewBag.CancelledCount = await _context.Appointments.CountAsync(a => a.Status == AppointmentStatus.Cancelled);

            return View(appointments);
        }

        // GET: Admin/Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Admin/Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            ViewBag.Services = new SelectList(
                await _context.Services.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync(),
                "Id", "Name", appointment.ServiceId);

            ViewBag.Statuses = new SelectList(
                Enum.GetValues<AppointmentStatus>()
                    .Select(s => new { Value = s, Text = s switch {
                        AppointmentStatus.Pending    => "Чакащ",
                        AppointmentStatus.Confirmed  => "Потвърден",
                        AppointmentStatus.InProgress => "В процес",
                        AppointmentStatus.Completed  => "Завършен",
                        AppointmentStatus.Cancelled  => "Отказан",
                        _ => s.ToString()
                    }}),
                "Value", "Text", appointment.Status);

            return View(appointment);
        }

        // POST: Admin/Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment model)
        {
            if (id != model.Id) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.CustomerName = model.CustomerName;
            appointment.Phone = model.Phone;
            appointment.Email = model.Email;
            appointment.CarBrand = model.CarBrand;
            appointment.CarModel = model.CarModel;
            appointment.CarYear = model.CarYear;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.Notes = model.Notes;
            appointment.ServiceId = model.ServiceId;
            appointment.Status = model.Status;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Записването на {appointment.CustomerName} е обновено!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Appointments/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.Status = status;
            await _context.SaveChangesAsync();

            var statusText = status switch
            {
                AppointmentStatus.Pending    => "Чакащ",
                AppointmentStatus.Confirmed  => "Потвърден",
                AppointmentStatus.InProgress => "В процес",
                AppointmentStatus.Completed  => "Завършен",
                AppointmentStatus.Cancelled  => "Отказан",
                _ => status.ToString()
            };

            TempData["SuccessMessage"] = $"Статусът е променен на \"{statusText}\".";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Admin/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Записването е изтрито успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}
