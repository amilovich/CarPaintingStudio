using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        // GET: Services — публично
        public async Task<IActionResult> Index(ServiceFilterViewModel filter)
        {
            var services = await _serviceService.GetServicesAsync(filter);
            ViewBag.Filter = filter;
            return View(services);
        }

        // GET: Services/Details/5 — публично
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetByIdWithReviewsAsync(id.Value);
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

            await _serviceService.CreateAsync(model);
            TempData["SuccessMessage"] = "Услугата е създадена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Edit/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null) return NotFound();

            var viewModel = new ServiceViewModel
            {
                Id           = service.Id,
                Name         = service.Name,
                Description  = service.Description,
                Price        = service.Price,
                DurationDays = service.DurationDays,
                IsActive     = service.IsActive
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

            var updated = await _serviceService.UpdateAsync(id, model);
            if (!updated) return NotFound();

            TempData["SuccessMessage"] = "Услугата е обновена успешно!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Delete/5 — само Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Services/Delete/5 — само Admin
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _serviceService.DeleteAsync(id);
            if (!deleted)
            {
                TempData["ErrorMessage"] = "Не може да изтриете услуга със свързани записвания!";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Услугата е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }
    }
}
