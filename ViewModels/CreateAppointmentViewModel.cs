using System.ComponentModel.DataAnnotations;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.ViewModels
{
    public class CreateAppointmentViewModel
    {
        [Required(ErrorMessage = "Името на клиента е задължително")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Името трябва да е между 2 и 100 символа")]
        [Display(Name = "Име на клиент")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Марката на автомобила е задължителна")]
        [Display(Name = "Марка")]
        public string CarBrand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моделът на автомобила е задължителен")]
        [Display(Name = "Модел")]
        public string CarModel { get; set; } = string.Empty;

        [Display(Name = "Година")]
        [Range(1950, 2030, ErrorMessage = "Годината трябва да е между 1950 и 2030")]
        public int? CarYear { get; set; }

        [Required(ErrorMessage = "Датата на записване е задължителна")]
        [Display(Name = "Дата на записване")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Забележки")]
        [StringLength(500)]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Моля изберете услуга")]
        [Display(Name = "Услуга")]
        public int ServiceId { get; set; }

        public List<Service>? AvailableServices { get; set; }
    }
}
