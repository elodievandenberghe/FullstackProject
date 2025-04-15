namespace BookingSite.ViewModels;

public class RouteViewModel
{
    public int Id { get; set; }
    public int? FromAirportId { get; set; }
    public int? ToAirportId { get; set; }
    public double? Price { get; set; }
    public string? FromAirportName { get; set; }
    public string? ToAirportName { get; set; }
}
