using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSite.ViewModels
{
    public class MultiTicketPurchaseModel
    {
        public int FlightId { get; set; }
        
        public List<TicketItem> Tickets { get; set; } = new List<TicketItem>();
    }

    public class TicketItem
    {
        [Required(ErrorMessage = "Please select a travel class")]
        public string SeatClass { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please select a meal")]
        public string Meal { get; set; } = string.Empty;
    }
}
