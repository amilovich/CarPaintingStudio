using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments — само логнати потребители виждат всички записвания
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/Details/5 — само логнати
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create — достъпно за всички (и гости)
        public async Task<IActionResult> Create()
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            var viewModel = new CreateAppointmentViewModel
            {
                AvailableServices = services,
                AppointmentDate = DateTime.Now.AddDays(1)
            };

            // Попълни данните автоматично ако е логнат
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    viewModel.CustomerName = user.FullName ?? string.Empty;
                    viewModel.Email = user.Email ?? string.Empty;
                }
            }

            return View(viewModel);
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentViewModel model)
        {
            if (model.AppointmentDate <= DateTime.Now)
            {
                ModelState.AddModelError("AppointmentDate", "Датата на записване трябва да е в бъдещето");
            }

            if (!ModelState.IsValid)
            {
                model.AvailableServices = await _context.Services
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();
                return View(model);
            }

            var appointment = new Appointment
            {
                CustomerName = model.CustomerName,
                Phone = model.Phone,
                Email = model.Email,
                CarBrand = model.CarBrand,
                CarModel = model.CarModel,
                CarYear = model.CarYear,
                AppointmentDate = model.AppointmentDate,
                Notes = model.Notes,
                ServiceId = model.ServiceId,
                Status = AppointmentStatus.Pending,
                CreatedDate = DateTime.Now
            };

            // Свърже с потребителя ако е логнат
            if (User.Identity?.IsAuthenticated == true)
            {
                appointment.UserId = _userManager.GetUserId(User);
            }

            _context.Add(appointment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Вашето записване е създадено успешно! Ще се свържем с Вас скоро.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Edit/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewData["ServiceId"] = new SelectList(
                await _context.Services.Where(s => s.IsActive).ToListAsync(),
                "Id", "Name", appointment.ServiceId);

            return View(appointment);
        }

        // POST: Appointments/Edit/5 — само Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Записването е обновено успешно!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appointments.Any(e => e.Id == appointment.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ServiceId"] = new SelectList(
                await _context.Services.Where(s => s.IsActive).ToListAsync(),
                "Id", "Name", appointment.ServiceId);

            return View(appointment);
        }

        // GET: Appointments/Delete/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5 — само Admin
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Записването е изтрито успешно!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
