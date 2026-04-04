using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services — публично
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View(services);
        }

        // GET: Services/Details/5 — публично
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Reviews.Where(r => r.IsApproved))
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);

            if (service == null) return NotFound();

            return View(service);
        }

        // GET: Services/Create — само Admin
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new ServiceViewModel());
        }

        // POST: Services/Create — само Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = new Service
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DurationDays = model.DurationDays,
                IsActive = model.IsActive,
                CreatedDate = DateTime.Now
            };

            _context.Add(service);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Услугата е създадена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Edit/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            var viewModel = new ServiceViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationDays = service.DurationDays,
                IsActive = service.IsActive
            };

            return View(viewModel);
        }

        // POST: Services/Edit/5 — само Admin
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            service.Name = model.Name;
            service.Description = model.Description;
            service.Price = model.Price;
            service.DurationDays = model.DurationDays;
            service.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Услугата е обновена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Delete/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5 — само Admin
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            if (service.Appointments.Any())
            {
                TempData["ErrorMessage"] = "Не може да изтриете услуга със свързани записвания!";
                return RedirectToAction(nameof(Index));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Услугата е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}
