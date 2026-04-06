namespace CarPaintingStudio.ViewModels
{
    public class ReviewFilterViewModel
    {
        public string? Search { get; set; }
        public int? MinRating { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
