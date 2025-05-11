using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookingSite.ViewModels;

public class TicketOverviewViewModel : TicketPurchaseInputModel
{
    public string FromAirport { get; set; }

    public string ToAirport { get; set; }

    public List<string> RouteSegments { get; set; }

    public DateOnly? Date { get; set; }

    public SelectList Meals { get; set; }

    public List<SeatClassViewModel> SeatClasses { get; set; }

    public double Price { get; set; }
}