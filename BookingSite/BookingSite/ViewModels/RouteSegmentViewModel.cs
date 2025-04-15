namespace BookingSite.ViewModels;

public class RouteSegmentViewModel
{
    public int Id { get; set; }
    public int? RouteId { get; set; }
    public int? SequenceNumber { get; set; }
    public int? AirportId { get; set; }
    public string? AirportName { get; set; }
}
