using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.HasMany(e => e.Appointments)
                      .WithOne(e => e.Service)
                      .HasForeignKey(e => e.ServiceId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Reviews)
                      .WithOne(e => e.Service)
                      .HasForeignKey(e => e.ServiceId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<GalleryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Service)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.ServiceId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // ── Услуги ──────────────────────────────────────────────────────
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Пълно боядисване",
                    Description = "Пълно боядисване на автомобила във всякакъв цвят по избор",
                    Price = 2500m, DurationDays = 7, IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1) },
                new Service { Id = 2, Name = "Частично боядисване",
                    Description = "Боядисване на отделни елементи - врати, калници, капаци",
                    Price = 800m, DurationDays = 3, IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1) },
                new Service { Id = 3, Name = "Полиране и защита",
                    Description = "Професионално полиране и нанасяне на защитно покритие",
                    Price = 450m, DurationDays = 2, IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1) },
                new Service { Id = 4, Name = "Матово боядисване",
                    Description = "Специално матово покритие за уникален външен вид",
                    Price = 3200m, DurationDays = 8, IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1) },
                new Service { Id = 5, Name = "Керамично покритие",
                    Description = "Нанасяне на керамично покритие за дълготрайна защита на боята",
                    Price = 1200m, DurationDays = 3, IsActive = true,
                    CreatedDate = new DateTime(2025, 2, 1) },
                new Service { Id = 6, Name = "Vinyl wrap",
                    Description = "Цялостно или частично фолиране с висококачествен vinyl",
                    Price = 2800m, DurationDays = 5, IsActive = true,
                    CreatedDate = new DateTime(2025, 2, 1) }
            );

            // ── Служители ───────────────────────────────────────────────────
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FullName = "Иван Георгиев",
                    Position = "Главен боядисвач",
                    Email = "ivan.georgiev@carpaint.bg", Phone = "0888123456",
                    YearsOfExperience = 15, IsActive = true,
                    HireDate = new DateTime(2010, 3, 15),
                    Bio = "Специалист с над 15 години опит в автомобилното боядисване" },
                new Employee { Id = 2, FullName = "Петър Димитров",
                    Position = "Боядисвач",
                    Email = "petar.dimitrov@carpaint.bg", Phone = "0888234567",
                    YearsOfExperience = 8, IsActive = true,
                    HireDate = new DateTime(2017, 6, 1),
                    Bio = "Експерт по матови покрития и специални ефекти" },
                new Employee { Id = 3, FullName = "Стоян Николов",
                    Position = "Vinyl специалист",
                    Email = "stoyan.nikolov@carpaint.bg", Phone = "0888345678",
                    YearsOfExperience = 5, IsActive = true,
                    HireDate = new DateTime(2020, 9, 1),
                    Bio = "Специалист по vinyl wrap и керамични покрития" },
                new Employee { Id = 4, FullName = "Димитър Василев",
                    Position = "Полировчик",
                    Email = "dimitar.vasilev@carpaint.bg", Phone = "0888456789",
                    YearsOfExperience = 6, IsActive = true,
                    HireDate = new DateTime(2019, 4, 15),
                    Bio = "Майстор на полирането и детайлинга" }
            );

            // ── Галерия ─────────────────────────────────────────────────────
            modelBuilder.Entity<GalleryItem>().HasData(
                new GalleryItem { Id = 1, Title = "Mercedes E-Class - Перлено бяло",
                    Description = "Пълно боядисване в перлено бяло със защитно покритие",
                    CarBrand = "Mercedes", CarModel = "E-Class",
                    BeforeImageUrl = "/images/gallery/merc-before.jpg",
                    AfterImageUrl  = "/images/gallery/merc-after.jpg",
                    CompletedDate = new DateTime(2024, 12, 15),
                    IsVisible = true, CreatedDate = new DateTime(2025, 1, 1) },
                new GalleryItem { Id = 2, Title = "BMW M3 - Матово черно",
                    Description = "Специално матово покритие с керамична защита",
                    CarBrand = "BMW", CarModel = "M3",
                    BeforeImageUrl = "/images/gallery/bmw-before.jpg",
                    AfterImageUrl  = "/images/gallery/bmw-after.jpg",
                    CompletedDate = new DateTime(2025, 1, 10),
                    IsVisible = true, CreatedDate = new DateTime(2025, 1, 1) },
                new GalleryItem { Id = 3, Title = "Audi A6 - Синьо металик",
                    Description = "Пълно боядисване в синьо металик с керамична защита",
                    CarBrand = "Audi", CarModel = "A6",
                    BeforeImageUrl = "/images/gallery/audi-before.jpg",
                    AfterImageUrl  = "/images/gallery/audi-after.jpg",
                    CompletedDate = new DateTime(2025, 1, 25),
                    IsVisible = true, CreatedDate = new DateTime(2025, 1, 25) },
                new GalleryItem { Id = 4, Title = "Porsche 911 - Vinyl wrap червено",
                    Description = "Цялостно фолиране с червен vinyl и гланц финиш",
                    CarBrand = "Porsche", CarModel = "911",
                    BeforeImageUrl = "/images/gallery/porsche-before.jpg",
                    AfterImageUrl  = "/images/gallery/porsche-after.jpg",
                    CompletedDate = new DateTime(2025, 2, 8),
                    IsVisible = true, CreatedDate = new DateTime(2025, 2, 8) },
                new GalleryItem { Id = 5, Title = "VW Golf - Полиране и защита",
                    Description = "Profesionalno полиране и нанасяне на керамично покритие",
                    CarBrand = "Volkswagen", CarModel = "Golf",
                    BeforeImageUrl = "/images/gallery/vw-before.jpg",
                    AfterImageUrl  = "/images/gallery/vw-after.jpg",
                    CompletedDate = new DateTime(2025, 2, 20),
                    IsVisible = true, CreatedDate = new DateTime(2025, 2, 20) },
                new GalleryItem { Id = 6, Title = "Toyota Supra - Матово сиво",
                    Description = "Матово сиво покритие с vinyl акценти",
                    CarBrand = "Toyota", CarModel = "Supra",
                    BeforeImageUrl = "/images/gallery/toyota-before.jpg",
                    AfterImageUrl  = "/images/gallery/toyota-after.jpg",
                    CompletedDate = new DateTime(2025, 3, 5),
                    IsVisible = true, CreatedDate = new DateTime(2025, 3, 5) }
            );

            // ── Записвания ──────────────────────────────────────────────────
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { Id = 1,
                    CustomerName = "Георги Петров", Phone = "0877111222",
                    Email = "georgi@example.com", CarBrand = "Toyota",
                    CarModel = "Camry", CarYear = 2020,
                    AppointmentDate = new DateTime(2025, 3, 10),
                    ServiceId = 1, Status = AppointmentStatus.Completed,
                    Notes = "Предпочита тъмно синьо",
                    CreatedDate = new DateTime(2025, 3, 1) },
                new Appointment { Id = 2,
                    CustomerName = "Мария Иванова", Phone = "0877222333",
                    Email = "maria@example.com", CarBrand = "Honda",
                    CarModel = "Civic", CarYear = 2019,
                    AppointmentDate = new DateTime(2025, 3, 15),
                    ServiceId = 3, Status = AppointmentStatus.Completed,
                    CreatedDate = new DateTime(2025, 3, 5) },
                new Appointment { Id = 3,
                    CustomerName = "Николай Стоянов", Phone = "0877333444",
                    Email = "nikolay@example.com", CarBrand = "Ford",
                    CarModel = "Focus", CarYear = 2021,
                    AppointmentDate = new DateTime(2025, 4, 2),
                    ServiceId = 2, Status = AppointmentStatus.Confirmed,
                    CreatedDate = new DateTime(2025, 3, 20) },
                new Appointment { Id = 4,
                    CustomerName = "Елена Димитрова", Phone = "0877444555",
                    Email = "elena@example.com", CarBrand = "Kia",
                    CarModel = "Sportage", CarYear = 2022,
                    AppointmentDate = new DateTime(2025, 4, 10),
                    ServiceId = 5, Status = AppointmentStatus.Pending,
                    Notes = "Иска керамично покритие на цялото тяло",
                    CreatedDate = new DateTime(2025, 3, 25) },
                new Appointment { Id = 5,
                    CustomerName = "Стефан Колев", Phone = "0877555666",
                    Email = "stefan@example.com", CarBrand = "BMW",
                    CarModel = "X5", CarYear = 2023,
                    AppointmentDate = new DateTime(2025, 4, 18),
                    ServiceId = 4, Status = AppointmentStatus.Pending,
                    Notes = "Матово черно - целия автомобил",
                    CreatedDate = new DateTime(2025, 3, 28) }
            );

            // ── Отзиви ───────────────────────────────────────────────────────
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, AuthorName = "Георги Петров",
                    AuthorEmail = "georgi@example.com",
                    Content = "Страхотна работа! Колата изглежда като нова. Боядисаха я перфектно и в срок. Препоръчвам на всички!",
                    Rating = 5, IsApproved = true, ServiceId = 1,
                    CreatedDate = new DateTime(2025, 3, 12) },
                new Review { Id = 2, AuthorName = "Мария Иванова",
                    AuthorEmail = "maria@example.com",
                    Content = "Много съм доволна от резултата. Полирането е страхотно - колата свети! Екипът е много любезен и професионален.",
                    Rating = 5, IsApproved = true, ServiceId = 3,
                    CreatedDate = new DateTime(2025, 3, 17) },
                new Review { Id = 3, AuthorName = "Николай Стоянов",
                    AuthorEmail = "nikolay@example.com",
                    Content = "Добра работа, доволен съм. Малко по-дълго от очакваното но резултатът си заслужава.",
                    Rating = 4, IsApproved = true, ServiceId = 2,
                    CreatedDate = new DateTime(2025, 2, 18) },
                new Review { Id = 4, AuthorName = "Александра Тодорова",
                    AuthorEmail = "alex@example.com",
                    Content = "Невероятен резултат с vinyl wrap-а! Колата изглежда като от шоурум. Ще се върна определено.",
                    Rating = 5, IsApproved = true, ServiceId = null,
                    CreatedDate = new DateTime(2025, 2, 25) },
                new Review { Id = 5, AuthorName = "Тест Потребител",
                    AuthorEmail = "test@example.com",
                    Content = "Все още чака одобрение - тестов отзив.",
                    Rating = 3, IsApproved = false, ServiceId = null,
                    CreatedDate = new DateTime(2025, 3, 30) }
            );
        }
    }
}
