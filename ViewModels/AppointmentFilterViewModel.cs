namespace CarPaintingStudio.ViewModels
{
    public class AppointmentFilterViewModel
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
    }
}
