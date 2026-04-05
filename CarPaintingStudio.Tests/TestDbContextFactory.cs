using Microsoft.EntityFrameworkCore;
using CarPaintingStudio.Data;
using CarPaintingStudio.Models;

namespace CarPaintingStudio.Tests
{
    public static class TestDbContextFactory
    {
        // Създава чист контекст БЕЗ да вика EnsureCreated (избягва seed от OnModelCreating)
        public static ApplicationDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext CreateWithSeedData(string dbName)
        {
            var context = CreateInMemoryContext(dbName);

            // Seed Services
            var svc1 = new Service
            {
                Name = "Пълно боядисване",
                Description = "Пълно боядисване на автомобила",
                Price = 2500m, DurationDays = 7,
                IsActive = true, CreatedDate = new DateTime(2025, 1, 1)
            };
            var svc2 = new Service
            {
                Name = "Полиране",
                Description = "Професионално полиране",
                Price = 450m, DurationDays = 2,
                IsActive = true, CreatedDate = new DateTime(2025, 1, 1)
            };
            var svc3 = new Service
            {
                Name = "Неактивна услуга",
                Description = "Тази услуга е неактивна",
                Price = 100m, DurationDays = 1,
                IsActive = false, CreatedDate = new DateTime(2025, 1, 1)
            };

            context.Services.AddRange(svc1, svc2, svc3);
            context.SaveChanges();

            // Seed Appointments
            context.Appointments.AddRange(
                new Appointment
                {
                    CustomerName = "Иван Петров",
                    Phone = "0888111111", Email = "ivan@test.com",
                    CarBrand = "BMW", CarModel = "M3", CarYear = 2020,
                    AppointmentDate = new DateTime(2025, 4, 10),
                    ServiceId = svc1.Id, Status = AppointmentStatus.Pending,
                    CreatedDate = new DateTime(2025, 3, 1)
                },
                new Appointment
                {
                    CustomerName = "Мария Иванова",
                    Phone = "0888222222", Email = "maria@test.com",
                    CarBrand = "Audi", CarModel = "A4", CarYear = 2021,
                    AppointmentDate = new DateTime(2025, 4, 15),
                    ServiceId = svc2.Id, Status = AppointmentStatus.Confirmed,
                    CreatedDate = new DateTime(2025, 3, 5)
                },
                new Appointment
                {
                    CustomerName = "Петър Георгиев",
                    Phone = "0888333333", Email = "petar@test.com",
                    CarBrand = "Mercedes", CarModel = "C200",
                    AppointmentDate = new DateTime(2025, 5, 1),
                    ServiceId = svc1.Id, Status = AppointmentStatus.Completed,
                    CreatedDate = new DateTime(2025, 3, 10)
                }
            );

            // Seed Reviews
            context.Reviews.AddRange(
                new Review
                {
                    AuthorName = "Иван Петров", AuthorEmail = "ivan@test.com",
                    Content = "Страхотна услуга, много доволен!",
                    Rating = 5, IsApproved = true,
                    ServiceId = svc1.Id, CreatedDate = new DateTime(2025, 2, 1)
                },
                new Review
                {
                    AuthorName = "Мария Иванова", AuthorEmail = "maria@test.com",
                    Content = "Много добра работа.",
                    Rating = 4, IsApproved = true,
                    ServiceId = svc2.Id, CreatedDate = new DateTime(2025, 2, 15)
                },
                new Review
                {
                    AuthorName = "Нов потребител", AuthorEmail = "new@test.com",
                    Content = "Чакащ одобрение.",
                    Rating = 3, IsApproved = false,
                    CreatedDate = new DateTime(2025, 3, 1)
                }
            );

            context.SaveChanges();
            return context;
        }
    }
}
