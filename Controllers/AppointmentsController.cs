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
        private const int PageSize = 8;

        public AppointmentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Appointments — само логнати, с pagination и search/filter
        [Authorize]
        public async Task<IActionResult> Index(AppointmentFilterViewModel filter)
        {
            filter.Page = filter.Page < 1 ? 1 : filter.Page;

            var query = _context.Appointments
                .Include(a => a.Service)
                .AsQueryable();

            // Само своите записвания ако не е Admin
            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(a => a.UserId == userId);
            }

            // Търсене по клиент, марка, модел, имейл
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var s = filter.Search.ToLower();
                query = query.Where(a =>
                    a.CustomerName.ToLower().Contains(s) ||
                    a.CarBrand.ToLower().Contains(s) ||
                    a.CarModel.ToLower().Contains(s) ||
                    a.Email.ToLower().Contains(s));
            }

            // Филтър по статус
            if (!string.IsNullOrEmpty(filter.Status) &&
                Enum.TryParse<AppointmentStatus>(filter.Status, out var parsedStatus))
            {
                query = query.Where(a => a.Status == parsedStatus);
            }

            // Филтър по период
            if (filter.DateFrom.HasValue)
                query = query.Where(a => a.AppointmentDate >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                query = query.Where(a => a.AppointmentDate <= filter.DateTo.Value);

            query = query.OrderByDescending(a => a.AppointmentDate);

            var paginated = await PaginatedList<Appointment>
                .CreateAsync(query, filter.Page, PageSize);

            // Статистика за всички (само Admin вижда всички)
            var baseQuery = _context.Appointments.AsQueryable();
            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                baseQuery = baseQuery.Where(a => a.UserId == userId);
            }

            ViewBag.PendingCount   = await baseQuery.CountAsync(a => a.Status == AppointmentStatus.Pending);
            ViewBag.ConfirmedCount = await baseQuery.CountAsync(a => a.Status == AppointmentStatus.Confirmed);
            ViewBag.InProgressCount= await baseQuery.CountAsync(a => a.Status == AppointmentStatus.InProgress);
            ViewBag.CompletedCount = await baseQuery.CountAsync(a => a.Status == AppointmentStatus.Completed);
            ViewBag.Filter = filter;

            return View(paginated);
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
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();

            var viewModel = new CreateAppointmentViewModel
            {
                AvailableServices = services,
                AppointmentDate = DateTime.Now.AddDays(1)
            };

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
                ModelState.AddModelError("AppointmentDate", "Датата трябва да е в бъдещето");

            if (!ModelState.IsValid)
            {
                model.AvailableServices = await _context.Services
                    .Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                return View(model);
            }

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
                Status          = AppointmentStatus.Pending,
                CreatedDate     = DateTime.Now
            };

            if (User.Identity?.IsAuthenticated == true)
                appointment.UserId = _userManager.GetUserId(User);

            _context.Add(appointment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Записването е създадено успешно! Ще се свържем с Вас скоро.";
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
