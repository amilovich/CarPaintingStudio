using System.ComponentModel.DataAnnotations;

namespace CarPaintingStudio.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Името трябва да е между 2 и 100 символа")]
        [Display(Name = "Име")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Позицията е задължителна")]
        [StringLength(100, ErrorMessage = "Позицията не може да надвишава 100 символа")]
        [Display(Name = "Позиция")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Години опит")]
        [Range(0, 50, ErrorMessage = "Годините опит трябва да са между 0 и 50")]
        public int YearsOfExperience { get; set; }

        [Display(Name = "Снимка")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Дата на наемане")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.Now;

        [Display(Name = "Биография")]
        [StringLength(1000, ErrorMessage = "Биографията не може да надвишава 1000 символа")]
        public string? Bio { get; set; }
    }
}
