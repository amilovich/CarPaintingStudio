using System.ComponentModel.DataAnnotations;

namespace CarPaintingStudio.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на услугата е задължително")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Името трябва да е между 3 и 100 символа")]
        [Display(Name = "Име на услугата")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описанието е задължително")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Описанието трябва да е между 10 и 500 символа")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цената е задължителна")]
        [Range(50, 10000, ErrorMessage = "Цената трябва да е между 50 и 10000 лв.")]
        [Display(Name = "Цена (лв)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Продължителността е задължителна")]
        [Range(1, 30, ErrorMessage = "Продължителността трябва да е между 1 и 30 дни")]
        [Display(Name = "Продължителност (дни)")]
        public int DurationDays { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Добавена на")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
