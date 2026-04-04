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

            // Service configuration
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

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // GalleryItem configuration
            modelBuilder.Entity<GalleryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // Review configuration
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Service)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.ServiceId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Services
            modelBuilder.Entity<Service>().HasData(
                new Service
                {
                    Id = 1,
                    Name = "Пълно боядисване",
                    Description = "Пълно боядисване на автомобила във всякакъв цвят по избор",
                    Price = 2500m,
                    DurationDays = 7,
                    IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new Service
                {
                    Id = 2,
                    Name = "Частично боядисване",
                    Description = "Боядисване на отделни елементи - врати, калници, капаци",
                    Price = 800m,
                    DurationDays = 3,
                    IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new Service
                {
                    Id = 3,
                    Name = "Полиране и защита",
                    Description = "Професионално полиране и нанасяне на защитно покритие",
                    Price = 450m,
                    DurationDays = 2,
                    IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new Service
                {
                    Id = 4,
                    Name = "Матово боядисване",
                    Description = "Специално матово покритие за уникален външен вид",
                    Price = 3200m,
                    DurationDays = 8,
                    IsActive = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                }
            );

            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FullName = "Иван Георгиев",
                    Position = "Главен боядисвач",
                    Email = "ivan.georgiev@carpaint.bg",
                    Phone = "0888123456",
                    YearsOfExperience = 15,
                    IsActive = true,
                    HireDate = new DateTime(2010, 3, 15),
                    Bio = "Специалист с над 15 години опит в автомобилното боядисване"
                },
                new Employee
                {
                    Id = 2,
                    FullName = "Петър Димитров",
                    Position = "Боядисвач",
                    Email = "petar.dimitrov@carpaint.bg",
                    Phone = "0888234567",
                    YearsOfExperience = 8,
                    IsActive = true,
                    HireDate = new DateTime(2017, 6, 1),
                    Bio = "Експерт по матови покрития и специални ефекти"
                }
            );

            // Seed Gallery Items
            modelBuilder.Entity<GalleryItem>().HasData(
                new GalleryItem
                {
                    Id = 1,
                    Title = "Mercedes E-Class - Перлено бяло",
                    Description = "Пълно боядисване в перлено бяло със защитно покритие",
                    CarBrand = "Mercedes",
                    CarModel = "E-Class",
                    BeforeImageUrl = "/images/gallery/merc-before.jpg",
                    AfterImageUrl = "/images/gallery/merc-after.jpg",
                    CompletedDate = new DateTime(2024, 12, 15),
                    IsVisible = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                },
                new GalleryItem
                {
                    Id = 2,
                    Title = "BMW M3 - Матово черно",
                    Description = "Специално матово покритие с керамична защита",
                    CarBrand = "BMW",
                    CarModel = "M3",
                    BeforeImageUrl = "/images/gallery/bmw-before.jpg",
                    AfterImageUrl = "/images/gallery/bmw-after.jpg",
                    CompletedDate = new DateTime(2025, 1, 10),
                    IsVisible = true,
                    CreatedDate = new DateTime(2025, 1, 1)
                }
            );

            // Seed Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    AuthorName = "Георги Петров",
                    AuthorEmail = "georgi@example.com",
                    Content = "Страхотна работа! Колата изглежда като нова. Препоръчвам на всички!",
                    Rating = 5,
                    IsApproved = true,
                    ServiceId = 1,
                    CreatedDate = new DateTime(2025, 1, 20)
                },
                new Review
                {
                    Id = 2,
                    AuthorName = "Мария Иванова",
                    AuthorEmail = "maria@example.com",
                    Content = "Много съм доволна от резултата. Професионален екип и отлично качество.",
                    Rating = 5,
                    IsApproved = true,
                    ServiceId = 3,
                    CreatedDate = new DateTime(2025, 2, 5)
                },
                new Review
                {
                    Id = 3,
                    AuthorName = "Николай Стоянов",
                    AuthorEmail = "nikolay@example.com",
                    Content = "Добра работа, доволен съм. Малко по-дълго от очакваното, но резултатът си заслужава.",
                    Rating = 4,
                    IsApproved = true,
                    ServiceId = 2,
                    CreatedDate = new DateTime(2025, 2, 18)
                }
            );
        }
    }
}
