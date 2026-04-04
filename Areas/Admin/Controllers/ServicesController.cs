using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.Appointments)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View(services);
        }

        // GET: Admin/Services/Create
        public IActionResult Create()
        {
            return View(new ServiceViewModel());
        }

        // POST: Admin/Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

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
            TempData["SuccessMessage"] = $"Услугата \"{service.Name}\" е създадена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            var model = new ServiceViewModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                DurationDays = service.DurationDays,
                IsActive = service.IsActive
            };

            return View(model);
        }

        // POST: Admin/Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            service.Name = model.Name;
            service.Description = model.Description;
            service.Price = model.Price;
            service.DurationDays = model.DurationDays;
            service.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Услугата \"{service.Name}\" е обновена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services
                .Include(s => s.Appointments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            if (service.Appointments.Any())
            {
                TempData["ErrorMessage"] = "Не може да изтриете услуга, която има свързани записвания!";
                return RedirectToAction(nameof(Index));
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Услугата \"{service.Name}\" е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Services/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            service.IsActive = !service.IsActive;
            await _context.SaveChangesAsync();

            var status = service.IsActive ? "активирана" : "деактивирана";
            TempData["SuccessMessage"] = $"Услугата \"{service.Name}\" е {status}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
