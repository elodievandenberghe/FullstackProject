using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingSite.ViewModels
{
    public class FlightsOverviewViewModel
    {
        public List<FlightViewModel> Flights { get; set; } = new List<FlightViewModel>();
        public List<SelectListItem> Airports { get; set; } = new List<SelectListItem>();

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // Filter properties
        public int? FromAirportId { get; set; }
        public int? ToAirportId { get; set; }
        public DateTime? DepartureDate { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
