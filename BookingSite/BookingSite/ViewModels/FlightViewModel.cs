namespace BookingSite.ViewModels;

public class FlightViewModel
{
    public int Id { get; set; }
    public int? RouteId { get; set; }
    public DateOnly? Date { get; set; }
    public string? RouteDisplay { get; set; }
}
