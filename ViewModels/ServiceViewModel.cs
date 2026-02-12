using System.ComponentModel.DataAnnotations;

namespace CarPaintingStudio.ViewModels
{
    public class ServiceViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името на услугата е задължително")]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Име на услугата")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описанието е задължително")]
        [StringLength(500, MinimumLength = 10)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цената е задължителна")]
        [Range(50, 10000)]
        [Display(Name = "Цена (лв)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Продължителността е задължителна")]
        [Range(1, 30)]
        [Display(Name = "Продължителност (дни)")]
        public int DurationDays { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;
    }
}
