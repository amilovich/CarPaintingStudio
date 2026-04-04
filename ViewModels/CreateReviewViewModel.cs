using System.ComponentModel.DataAnnotations;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.ViewModels
{
    public class CreateReviewViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Името трябва да е между 2 и 100 символа")]
        [Display(Name = "Вашето име")]
        public string AuthorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string AuthorEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Съдържанието е задължително")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Отзивът трябва да е между 10 и 1000 символа")]
        [Display(Name = "Вашият отзив")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Оценката е задължителна")]
        [Range(1, 5, ErrorMessage = "Оценката трябва да е между 1 и 5")]
        [Display(Name = "Оценка")]
        public int Rating { get; set; } = 5;

        [Display(Name = "Услуга (по избор)")]
        public int? ServiceId { get; set; }

        // За попълване на dropdown
        public IEnumerable<Service> AvailableServices { get; set; } = new List<Service>();
    }
}
