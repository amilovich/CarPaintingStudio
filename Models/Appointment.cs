using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPaintingStudio.Models
{
    public class Appointment
    {
        public int Id { get; set; }

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
        [StringLength(50, ErrorMessage = "Марката не може да надвишава 50 символа")]
        [Display(Name = "Марка")]
        public string CarBrand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моделът на автомобила е задължителен")]
        [StringLength(50, ErrorMessage = "Моделът не може да надвишава 50 символа")]
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
        [StringLength(500, ErrorMessage = "Забележките не могат да надвишават 500 символа")]
        public string? Notes { get; set; }

        [Display(Name = "Статус")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [Required]
        [Display(Name = "Услуга")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        [Display(Name = "Създаден на")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public enum AppointmentStatus
    {
        [Display(Name = "Чакащ")]
        Pending,
        [Display(Name = "Потвърден")]
        Confirmed,
        [Display(Name = "В процес")]
        InProgress,
        [Display(Name = "Завършен")]
        Completed,
        [Display(Name = "Отказан")]
        Cancelled
    }
}
