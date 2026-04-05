using Xunit;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Tests.Services
{
    public class ServiceServiceTests
    {
        private static (CarPaintingStudio.Services.ServiceService svc, int id1, int id2) Create()
        {
            var db      = Guid.NewGuid().ToString();
            var context = TestDbContextFactory.CreateWithSeedData(db);
            var id1     = context.Services.First(s => s.Name == "Пълно боядисване").Id;
            var id2     = context.Services.First(s => s.Name == "Полиране").Id;
            return (new CarPaintingStudio.Services.ServiceService(context), id1, id2);
        }

        [Fact]
        public async Task GetServicesAsync_ReturnsOnlyActiveServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel());
            Assert.All(result, s => Assert.True(s.IsActive));
        }

        [Fact]
        public async Task GetServicesAsync_SearchByName_ReturnsMatchingServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel { Search = "боядисване" });
            Assert.All(result, s => Assert.Contains("боядисване", s.Name.ToLower()));
        }

        [Fact]
        public async Task GetServicesAsync_FilterByMinPrice_ReturnsCorrectServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel { MinPrice = 1000m });
            Assert.All(result, s => Assert.True(s.Price >= 1000m));
        }

        [Fact]
        public async Task GetServicesAsync_FilterByMaxPrice_ReturnsCorrectServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel { MaxPrice = 500m });
            Assert.All(result, s => Assert.True(s.Price <= 500m));
        }

        [Fact]
        public async Task GetServicesAsync_SortByPriceAsc_ReturnsSortedServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel { SortBy = "price_asc" });
            var prices = result.Select(s => s.Price).ToList();
            Assert.Equal(prices.OrderBy(p => p), prices);
        }

        [Fact]
        public async Task GetServicesAsync_EmptySearch_ReturnsAllActiveServices()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetServicesAsync(new ServiceFilterViewModel { Search = "" });
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsService()
        {
            var (svc, id1, _) = Create();
            var result = await svc.GetByIdAsync(id1);
            Assert.NotNull(result);
            Assert.Equal("Пълно боядисване", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetByIdAsync(99999);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidModel_CreatesService()
        {
            var (svc, _, _) = Create();
            var model = new ServiceViewModel
            {
                Name = "Нова услуга", Description = "Описание на новата услуга тест",
                Price = 750m, DurationDays = 3, IsActive = true
            };
            var result = await svc.CreateAsync(model);
            Assert.NotNull(result);
            Assert.Equal("Нова услуга", result.Name);
            Assert.Equal(750m, result.Price);
        }

        [Fact]
        public async Task CreateAsync_SetsCreatedDateToNow()
        {
            var (svc, _, _) = Create();
            var model = new ServiceViewModel
            {
                Name = "Тест услуга", Description = "Описание за тест услуга дата",
                Price = 100m, DurationDays = 1, IsActive = true
            };
            var result = await svc.CreateAsync(model);
            Assert.True(result.CreatedDate >= DateTime.Now.AddMinutes(-1));
        }

        [Fact]
        public async Task UpdateAsync_ExistingId_UpdatesService()
        {
            var (svc, id1, _) = Create();
            var model = new ServiceViewModel
            {
                Id = id1, Name = "Обновено", Description = "Обновено описание на услугата тест",
                Price = 3000m, DurationDays = 10, IsActive = true
            };
            var result = await svc.UpdateAsync(id1, model);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var model = new ServiceViewModel
            {
                Name = "Несъществуваща", Description = "Описание", Price = 100m, DurationDays = 1
            };
            var result = await svc.UpdateAsync(99999, model);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ServiceWithNoAppointments_DeletesSuccessfully()
        {
            var db = Guid.NewGuid().ToString();
            var context = TestDbContextFactory.CreateInMemoryContext(db);
            context.Services.Add(new Service
            {
                Name = "За изтриване", Description = "Тест за изтриване услуга",
                Price = 100m, DurationDays = 1, IsActive = true, CreatedDate = DateTime.Now
            });
            context.SaveChanges();
            var toDelete = context.Services.First();
            var svc = new CarPaintingStudio.Services.ServiceService(context);
            var result = await svc.DeleteAsync(toDelete.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.DeleteAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task ToggleActiveAsync_ActiveService_BecomesInactive()
        {
            var (svc, id1, _) = Create();
            var toggled = await svc.ToggleActiveAsync(id1);
            var result  = await svc.GetByIdAsync(id1);
            Assert.True(toggled);
            Assert.False(result!.IsActive);
        }

        [Fact]
        public async Task ToggleActiveAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.ToggleActiveAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task ExistsAsync_ExistingId_ReturnsTrue()
        {
            var (svc, id1, _) = Create();
            var result = await svc.ExistsAsync(id1);
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.ExistsAsync(99999);
            Assert.False(result);
        }
    }
}
