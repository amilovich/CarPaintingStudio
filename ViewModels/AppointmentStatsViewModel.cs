namespace CarPaintingStudio.ViewModels
{
    public class AppointmentStatsViewModel
    {
        public int PendingCount { get; set; }
        public int ConfirmedCount { get; set; }
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int CancelledCount { get; set; }
        public int TotalCount => PendingCount + ConfirmedCount + InProgressCount + CompletedCount + CancelledCount;
    }
}
