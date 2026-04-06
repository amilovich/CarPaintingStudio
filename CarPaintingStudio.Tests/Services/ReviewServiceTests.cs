using Xunit;
using CarPaintingStudio.Models;
using CarPaintingStudio.Services;
using CarPaintingStudio.ViewModels;

namespace CarPaintingStudio.Tests.Services
{
    public class ReviewServiceTests
    {
        private static (ReviewService svc, int approvedId, int pendingId) Create()
        {
            var db         = Guid.NewGuid().ToString();
            var context    = TestDbContextFactory.CreateWithSeedData(db);
            var approvedId = context.Reviews.First(r => r.IsApproved).Id;
            var pendingId  = context.Reviews.First(r => !r.IsApproved).Id;
            return (new ReviewService(context), approvedId, pendingId);
        }

        // Помощен метод за празен филтър
        private static ReviewFilterViewModel EmptyFilter() => new ReviewFilterViewModel();

        [Fact]
        public async Task GetApprovedReviewsAsync_ReturnsOnlyApprovedReviews()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetApprovedReviewsAsync(EmptyFilter());
            Assert.All(result, r => Assert.True(r.IsApproved));
        }

        [Fact]
        public async Task GetApprovedReviewsAsync_ReturnsCorrectCount()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetApprovedReviewsAsync(EmptyFilter());
            Assert.Equal(2, result.TotalCount);
        }

        [Fact]
        public async Task GetApprovedReviewsAsync_OrderedByDateDescending()
        {
            var (svc, _, _) = Create();
            var result = (await svc.GetApprovedReviewsAsync(EmptyFilter())).ToList();
            Assert.True(result[0].CreatedDate >= result[1].CreatedDate);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsReview()
        {
            var (svc, approvedId, _) = Create();
            var result = await svc.GetByIdAsync(approvedId);
            Assert.NotNull(result);
            Assert.Equal(approvedId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var (svc, _, _) = Create();
            var result = await svc.GetByIdAsync(99999);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidModel_CreatesReview()
        {
            var (svc, _, _) = Create();
            var model = new CreateReviewViewModel
            {
                AuthorName = "Тест Потребител", AuthorEmail = "test@example.com",
                Content = "Страхотна услуга, определено препоръчвам!", Rating = 5
            };
            var result = await svc.CreateAsync(model, "user-123");
            Assert.NotNull(result);
            Assert.Equal("Тест Потребител", result.AuthorName);
            Assert.Equal(5, result.Rating);
            Assert.Equal("user-123", result.UserId);
        }

        [Fact]
        public async Task CreateAsync_NewReview_IsNotApprovedByDefault()
        {
            var (svc, _, _) = Create();
            var model = new CreateReviewViewModel
            {
                AuthorName = "Нов потребител", AuthorEmail = "new@example.com",
                Content = "Тестов отзив за проверка на статуса по подразбиране", Rating = 4
            };
            var result = await svc.CreateAsync(model, null);
            Assert.False(result.IsApproved);
        }

        [Fact]
        public async Task CreateAsync_SetsCreatedDateToNow()
        {
            var (svc, _, _) = Create();
            var model = new CreateReviewViewModel
            {
                AuthorName = "Дата Тест", AuthorEmail = "date@example.com",
                Content = "Тестов отзив за проверка на датата на създаване", Rating = 3
            };
            var result = await svc.CreateAsync(model, null);
            Assert.True(result.CreatedDate >= DateTime.Now.AddMinutes(-1));
        }

        [Fact]
        public async Task ApproveAsync_PendingReview_BecomesApproved()
        {
            var (svc, _, pendingId) = Create();
            var result = await svc.ApproveAsync(pendingId);
            var review = await svc.GetByIdAsync(pendingId);
            Assert.True(result);
            Assert.True(review!.IsApproved);
        }

        [Fact]
        public async Task ApproveAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.ApproveAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task RejectAsync_ApprovedReview_BecomesRejected()
        {
            var (svc, approvedId, _) = Create();
            var result = await svc.RejectAsync(approvedId);
            var review = await svc.GetByIdAsync(approvedId);
            Assert.True(result);
            Assert.False(review!.IsApproved);
        }

        [Fact]
        public async Task RejectAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.RejectAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingReview_DeletesSuccessfully()
        {
            var (svc, approvedId, _) = Create();
            var result = await svc.DeleteAsync(approvedId);
            var review = await svc.GetByIdAsync(approvedId);
            Assert.True(result);
            Assert.Null(review);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var (svc, _, _) = Create();
            var result = await svc.DeleteAsync(99999);
            Assert.False(result);
        }

        [Fact]
        public async Task GetStatsAsync_ReturnsCorrectApprovedCount()
        {
            var (svc, _, _) = Create();
            var stats = await svc.GetStatsAsync();
            Assert.Equal(2, stats.TotalApproved);
        }

        [Fact]
        public async Task GetStatsAsync_ReturnsCorrectPendingCount()
        {
            var (svc, _, _) = Create();
            var stats = await svc.GetStatsAsync();
            Assert.Equal(1, stats.PendingCount);
        }

        [Fact]
        public async Task GetStatsAsync_ReturnsCorrectAverageRating()
        {
            var (svc, _, _) = Create();
            var stats = await svc.GetStatsAsync();
            Assert.Equal(4.5, stats.AverageRating);
        }
    }
}
