using System.ComponentModel.DataAnnotations;

namespace BookingSite.ViewModels
{
    public class TicketPurchaseInputModel
    {
        public TicketPurchaseInputModel() { }

        public TicketPurchaseInputModel(int flightId)
        {
            FlightId = flightId;
        }

        [Required]
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Please select a meal.")]
        public string SelectedMeal { get; set; }

        [Required(ErrorMessage = "Please select a seat class.")]
        public string SelectedSeatClass { get; set; }
    }
}
