namespace BookingSite.ViewModels;

public class FlightViewModel
{
    public int Id { get; set; }
    public int? RouteId { get; set; }
    public DateOnly? Date { get; set; }
    public string? FromAirport { get; set; }
    public string? ToAirport { get; set; }
    public int ToAirportId { get; set; }
    public string? RouteSegments { get; set; }
    public double Price { get; set; }
    public int AvailableSeats { get; set; }

}
