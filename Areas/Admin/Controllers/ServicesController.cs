using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // GET: Admin/Services
        public async Task<IActionResult> Index()
        {
            var filter   = new ServiceFilterViewModel { PageSize = 100 };
            var services = await _serviceService.GetServicesAsync(filter);
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
            if (!ModelState.IsValid) return View(model);

            var service = await _serviceService.CreateAsync(model);
            TempData["SuccessMessage"] = $"Услугата \"{service.Name}\" е създадена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null) return NotFound();

            var model = new ServiceViewModel
            {
                Id           = service.Id,
                Name         = service.Name,
                Description  = service.Description,
                Price        = service.Price,
                DurationDays = service.DurationDays,
                IsActive     = service.IsActive
            };

            return View(model);
        }

        // POST: Admin/Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var updated = await _serviceService.UpdateAsync(id, model);
            if (!updated) return NotFound();

            TempData["SuccessMessage"] = $"Услугата \"{model.Name}\" е обновена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            var name    = service?.Name ?? "Услугата";

            var deleted = await _serviceService.DeleteAsync(id);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Не може да изтриете услуга с свързани записвания!";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = $"\"{name}\" е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Services/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var toggled = await _serviceService.ToggleActiveAsync(id);
            if (!toggled) return NotFound();

            TempData["SuccessMessage"] = "Статусът на услугата е променен.";
            return RedirectToAction(nameof(Index));
        }
    }
}
