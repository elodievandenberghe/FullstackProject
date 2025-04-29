namespace BookingSite.ViewModels;

public class CartItemViewModel
{
    public int FlightId { get; set; }
    public DateOnly? Date { get; set; }
    public string? FromAirport { get; set; }
    public string? ToAirport { get; set; }
    public string? RouteSegments { get; set; }
    public int MealId { get; set; }
    public string? MealDescription { get; set; }
    public int ClassId { get; set; }
    public string? ClassType { get; set; }
    public double Price { get; set; }
}