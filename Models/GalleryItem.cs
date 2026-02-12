using System.ComponentModel.DataAnnotations;

namespace CarPaintingStudio.Models
{
    public class GalleryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Заглавието трябва да е между 3 и 100 символа")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описанието е задължително")]
        [StringLength(500, ErrorMessage = "Описанието не може да надвишава 500 символа")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Снимка преди")]
        public string? BeforeImageUrl { get; set; }

        [Display(Name = "Снимка след")]
        public string? AfterImageUrl { get; set; }

        [Required(ErrorMessage = "Марката на автомобила е задължителна")]
        [StringLength(50, ErrorMessage = "Марката не може да надвишава 50 символа")]
        [Display(Name = "Марка")]
        public string CarBrand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моделът на автомобила е задължителен")]
        [StringLength(50, ErrorMessage = "Моделът не може да надвишава 50 символа")]
        [Display(Name = "Модел")]
        public string CarModel { get; set; } = string.Empty;

        [Display(Name = "Година на завършване")]
        [DataType(DataType.Date)]
        public DateTime CompletedDate { get; set; } = DateTime.Now;

        [Display(Name = "Видима")]
        public bool IsVisible { get; set; } = true;

        [Display(Name = "Добавена на")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
