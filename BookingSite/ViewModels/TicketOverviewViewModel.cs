using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingSite.ViewModels;

public class TicketOverviewViewModel
{
    public int FlightId { get; set; }
    public string FromAirport { get; set; }
    public string ToAirport { get; set; }
    public string RouteSegments { get; set; }
    public DateOnly? Date { get; set; }
    public SelectList Meals { get; set; }
    public string SelectedMeal { get; set; }
    public List<SeatClassViewModel> SeatClasses { get; set; }
    public string SelectedSeatClass { get; set; }
    public double Price { get; set; }
}