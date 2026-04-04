using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarPaintingStudio.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        [Display(Name = "Пълно име")]
        public string? FullName { get; set; }

        [Display(Name = "Дата на регистрация")]
        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
