namespace BookingSite.ViewModels
{
    public class BookingWithTicketsViewModel
    {
        public int BookingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public List<TicketPreviewViewModel> Tickets { get; set; } = new();
        public double TotalPrice => Tickets.Sum(t => t.Price ?? 0);
    }

    public class TicketPreviewViewModel
    {
        public int TicketId { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public DateTime Date { get; set; }
        public string SeatClass { get; set; }
        public string MealDescription { get; set; }
        public double? Price { get; set; }
        public bool IsCancelled { get; set; }
    }
}
