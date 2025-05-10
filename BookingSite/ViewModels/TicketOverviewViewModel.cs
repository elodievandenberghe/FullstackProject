using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookingSite.ViewModels;

public class TicketOverviewViewModel
{
    [Required]
    public int FlightId { get; set; }

    [Required]
    public string FromAirport { get; set; }

    [Required]
    public string ToAirport { get; set; }

    public List<string> RouteSegments { get; set; }

    [Required]
    public DateOnly? Date { get; set; }

    [Required(ErrorMessage = "Please select a meal.")]
    public string SelectedMeal { get; set; }

    [Required(ErrorMessage = "Please select a seat class.")]
    public string SelectedSeatClass { get; set; }

    public SelectList Meals { get; set; }

    public List<SeatClassViewModel> SeatClasses { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
    public double Price { get; set; }
}