namespace BookingSite.ViewModels;

public class TicketViewModel
{
    public int Id { get; set; }
    public int? FlightId { get; set; }
    public int? MealId { get; set; }
    public int? SeatNumber { get; set; }
    public string SeatClass { get; set; } = "Second Class";

    public bool? IsCancelled { get; set; }

    public string? FlightInfo { get; set; }
    public string? MealType { get; set; }
    public string? FormattedSeatInfo { get; set; }

}
