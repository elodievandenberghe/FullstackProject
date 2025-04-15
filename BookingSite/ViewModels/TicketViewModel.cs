namespace BookingSite.ViewModels;

public class TicketViewModel
{
    public int Id { get; set; }
    public int? FlightId { get; set; }
    public int? MealId { get; set; }
    public int? SeatId { get; set; }
    public bool? IsCancelled { get; set; }

    public string? FlightInfo { get; set; }
    public string? MealType { get; set; }
    public string? SeatNumber { get; set; }
}
