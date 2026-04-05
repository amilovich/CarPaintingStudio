using Xunit;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Tests.Services
{
    public class AppointmentServiceTests
    {
        private static (AppointmentService svc, int apt1Id, int svcId) Create()
        {
            var db    = Guid.NewGuid().ToString();
            var ctx   = TestDbContextFactory.CreateWithSeedData(db);
            var apt1  = ctx.Appointments.First(a => a.Status == AppointmentStatus.Pending);
            var svcId = ctx.Services.First(s => s.IsActive).Id;
            return (new AppointmentService(ctx), apt1.Id, svcId);
        }

        [Fact]
        public async Task GetAppointmentsAsync_Admin_ReturnsAllAppointments()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetAppointmentsAsync(new AppointmentFilterViewModel(), null, isAdmin: true);
            Assert.Equal(3, result.TotalCount);
        }

        [Fact]
        public async Task GetAppointmentsAsync_NonAdmin_ReturnsOnlyOwnAppointments()
        {
            var db  = Guid.NewGuid().ToString();
            var ctx = TestDbContextFactory.CreateInMemoryContext(db);
            ctx.Services.Add(new Service
            {
                Name = "Тест", Description = "Описание за тест услуга",
                Price = 100m, DurationDays = 1, IsActive = true, CreatedDate = DateTime.Now
            });
            ctx.SaveChanges();
            var sid = ctx.Services.First().Id;
            ctx.Appointments.AddRange(
                new Appointment
                {
                    CustomerName = "Мой клиент", Phone = "111", Email = "own@test.com",
                    CarBrand = "BMW", CarModel = "X5",
                    AppointmentDate = DateTime.Now.AddDays(5),
                    ServiceId = sid, Status = AppointmentStatus.Pending,
                    UserId = "user-1", CreatedDate = DateTime.Now
                },
                new Appointment
                {
                    CustomerName = "Чужд клиент", Phone = "222", Email = "other@test.com",
                    CarBrand = "Audi", CarModel = "A3",
                    AppointmentDate = DateTime.Now.AddDays(10),
                    ServiceId = sid, Status = AppointmentStatus.Pending,
                    UserId = "user-2", CreatedDate = DateTime.Now
                }
            );
            ctx.SaveChanges();
            var service = new AppointmentService(ctx);
            var result  = await service.GetAppointmentsAsync(new AppointmentFilterViewModel(), "user-1", isAdmin: false);
            Assert.Equal(1, result.TotalCount);
            Assert.All(result, a => Assert.Equal("user-1", a.UserId));
        }

        [Fact]
        public async Task GetAppointmentsAsync_FilterByStatus_ReturnsOnlyMatchingStatus()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetAppointmentsAsync(
                new AppointmentFilterViewModel { Status = "Pending" }, null, isAdmin: true);
            Assert.All(result, a => Assert.Equal(AppointmentStatus.Pending, a.Status));
        }

        [Fact]
        public async Task GetAppointmentsAsync_SearchByName_ReturnsMatchingAppointments()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetAppointmentsAsync(
                new AppointmentFilterViewModel { Search = "Иван" }, null, isAdmin: true);
            Assert.All(result, a => Assert.Contains("иван", a.CustomerName.ToLower()));
        }

        [Fact]
        public async Task GetAppointmentsAsync_FilterByDateFrom_ReturnsCorrectResults()
        {
            var (svc, _, _) = Create();
            var dateFrom = new DateTime(2025, 4, 14);
            var result = await svc.GetAppointmentsAsync(
                new AppointmentFilterViewModel { DateFrom = dateFrom }, null, isAdmin: true);
            Assert.All(result, a => Assert.True(a.AppointmentDate >= dateFrom));
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsAppointment()
        {
            var (svc, apt1Id, _) = Create();
            var result = await svc.GetByIdAsync(apt1Id);
            Assert.NotNull(result);
            Assert.Equal(apt1Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetByIdAsync(99999);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidModel_CreatesAppointment()
        {
            var (svc, _, svcId) = Create();
            var model = new CreateAppointmentViewModel
            {
                CustomerName = "Нов Клиент", Phone = "0888999888",
                Email = "new@test.com", CarBrand = "Toyota", CarModel = "Corolla",
                CarYear = 2022, AppointmentDate = DateTime.Now.AddDays(5), ServiceId = svcId
            };
            var result = await svc.CreateAsync(model, "user-new");
            Assert.NotNull(result);
            Assert.Equal("Нов Клиент", result.CustomerName);
            Assert.Equal(AppointmentStatus.Pending, result.Status);
            Assert.Equal("user-new", result.UserId);
        }

        [Fact]
        public async Task CreateAsync_SetsStatusToPending()
        {
            var (svc, _, svcId) = Create();
            var model = new CreateAppointmentViewModel
            {
                CustomerName = "Клиент Тест", Phone = "0888777666",
                Email = "status@test.com", CarBrand = "Ford", CarModel = "Focus",
                AppointmentDate = DateTime.Now.AddDays(3), ServiceId = svcId
            };
            var result = await svc.CreateAsync(model, null);
            Assert.Equal(AppointmentStatus.Pending, result.Status);
        }

        [Fact]
        public async Task ChangeStatusAsync_ValidId_ChangesStatus()
        {
            var (svc, apt1Id, _) = Create();
            var result      = await svc.ChangeStatusAsync(apt1Id, AppointmentStatus.Confirmed);
            var appointment = await svc.GetByIdAsync(apt1Id);
            Assert.True(result);
            Assert.Equal(AppointmentStatus.Confirmed, appointment!.Status);
        }

        [Fact]
        public async Task ChangeStatusAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.ChangeStatusAsync(99999, AppointmentStatus.Confirmed);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_DeletesAppointment()
        {
            var (svc, apt1Id, _) = Create();
            var result      = await svc.DeleteAsync(apt1Id);
            var appointment = await svc.GetByIdAsync(apt1Id);
            Assert.True(result);
            Assert.Null(appointment);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.DeleteAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task GetStatsAsync_Admin_ReturnsCorrectCounts()
        {
            var (svc, _, _) = Create();
            var stats = await svc.GetStatsAsync(null, isAdmin: true);
            Assert.Equal(1, stats.PendingCount);
            Assert.Equal(1, stats.ConfirmedCount);
            Assert.Equal(1, stats.CompletedCount);
            Assert.Equal(3, stats.TotalCount);
        }

        [Fact]
        public async Task GetStatsAsync_TotalCount_MatchesSumOfStatuses()
        {
            var (svc, _, _) = Create();
            var stats = await svc.GetStatsAsync(null, isAdmin: true);
            var manualTotal = stats.PendingCount + stats.ConfirmedCount +
                              stats.InProgressCount + stats.CompletedCount +
                              stats.CancelledCount;
            Assert.Equal(manualTotal, stats.TotalCount);
        }
    }
}
