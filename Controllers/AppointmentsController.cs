using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IServiceService serviceService,
            UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _serviceService     = serviceService;
            _userManager        = userManager;
        }

        // GET: Appointments — само логнати
        [Authorize]
        public async Task<IActionResult> Index(AppointmentFilterViewModel filter)
        {
            var userId  = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var paginated = await _appointmentService.GetAppointmentsAsync(filter, userId, isAdmin);
            var stats     = await _appointmentService.GetStatsAsync(userId, isAdmin);

            ViewBag.PendingCount    = stats.PendingCount;
            ViewBag.ConfirmedCount  = stats.ConfirmedCount;
            ViewBag.InProgressCount = stats.InProgressCount;
            ViewBag.CompletedCount  = stats.CompletedCount;
            ViewBag.Filter          = filter;

            return View(paginated);
        }

        // GET: Appointments/Details/5 — само логнати
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _appointmentService.GetByIdWithServiceAsync(id.Value);
            if (appointment == null) return NotFound();

            // Не-Admin виждат само своите
            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                if (appointment.UserId != userId) return Forbid();
            }

            return View(appointment);
        }

        // GET: Appointments/Create — достъпно за всички
        public async Task<IActionResult> Create()
        {
            var services = await _serviceService.GetActiveServicesAsync();

            var viewModel = new CreateAppointmentViewModel
            {
                AvailableServices = services.ToList(),
                AppointmentDate   = DateTime.Now.AddDays(1)
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    viewModel.CustomerName = user.FullName ?? string.Empty;
                    viewModel.Email        = user.Email    ?? string.Empty;
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
                ModelState.AddModelError("AppointmentDate", "Датата трябва да е в бъдещето");

            if (!ModelState.IsValid)
            {
                var services = await _serviceService.GetActiveServicesAsync();
                model.AvailableServices = services.ToList();
                return View(model);
            }

            var userId = User.Identity?.IsAuthenticated == true
                ? _userManager.GetUserId(User)
                : null;

            await _appointmentService.CreateAsync(model, userId);
            TempData["SuccessMessage"] = "Записването е създадено успешно! Ще се свържем с Вас скоро.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Edit/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment == null) return NotFound();

            var services = await _serviceService.GetActiveServicesAsync();
            ViewData["ServiceId"] = new SelectList(services, "Id", "Name", appointment.ServiceId);

            ViewBag.Statuses = new SelectList(
                Enum.GetValues<AppointmentStatus>()
                    .Select(s => new
                    {
                        Value = s,
                        Text  = s switch
                        {
                            AppointmentStatus.Pending    => "Чакащ",
                            AppointmentStatus.Confirmed  => "Потвърден",
                            AppointmentStatus.InProgress => "В процес",
                            AppointmentStatus.Completed  => "Завършен",
                            AppointmentStatus.Cancelled  => "Отказан",
                            _                            => s.ToString()
                        }
                    }),
                "Value", "Text", appointment.Status);

            return View(appointment);
        }

        // POST: Appointments/Edit/5 — само Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var services = await _serviceService.GetActiveServicesAsync();
                ViewData["ServiceId"] = new SelectList(services, "Id", "Name", model.ServiceId);
                return View(model);
            }

            var updated = await _appointmentService.UpdateAsync(id, model);
            if (!updated) return NotFound();

            TempData["SuccessMessage"] = "Записването е обновено успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Delete/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _appointmentService.GetByIdWithServiceAsync(id.Value);
            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointments/Delete/5 — само Admin
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Записването е изтрито успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}
