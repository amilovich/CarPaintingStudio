using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPaintingStudio.Models
{
    public class Review
    {
        public int Id { get; set; }

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
        public int Rating { get; set; }

        [Display(Name = "Одобрен")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Дата на публикуване")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Връзка към услуга (незадължителна)
        [Display(Name = "Услуга")]
        public int? ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        // Връзка към потребител (незадължителна)
        [Display(Name = "Потребител")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
