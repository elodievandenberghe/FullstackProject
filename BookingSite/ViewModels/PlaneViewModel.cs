namespace BookingSite.ViewModels
{
    public class PlaneViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
    }
}
