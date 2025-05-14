using BookingSite.Domains.Models;
using System.Collections.Generic;

namespace BookingSite.ViewModels
{
    public class BookingDetailsViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalPrice { get; set; }
        public List<BookingTicketViewModel> Tickets { get; set; } = new List<BookingTicketViewModel>();
    }

    public class BookingTicketViewModel
    {
        public int Id { get; set; }
        public int? SeatNumber { get; set; }
        public string SeatClassName { get; set; }
        public bool IsCancelled { get; set; }
        public double? Price { get; set; }
        public string MealDescription { get; set; }
        public BookingFlightViewModel Flight { get; set; }
    }

    public class BookingFlightViewModel
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public BookingRouteViewModel Route { get; set; }
    }

    public class BookingRouteViewModel
    {
        public AirportViewModel FromAirport { get; set; }
        public AirportViewModel ToAirport { get; set; }
    }
}
