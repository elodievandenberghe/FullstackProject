using BookingSite.Domains.Models;

namespace BookingSite.ViewModels;

public class CartItemViewModel
{
    public int FlightId { get; set; }
    public DateOnly? Date { get; set; }
    public string FromAirport { get; set; }
    public string ToAirport { get; set; }
    public List<string> RouteSegments { get; set; }
    public int MealId { get; set; }
    public SeatClass SeatClass { get; set; }
    public double Price { get; set; }
    public string? MealDescription { get; set; }

    public string SeatClassName => SeatClass == SeatClass.FirstClass ? "First Class" : "Second Class";
}