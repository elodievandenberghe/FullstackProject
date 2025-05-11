using System.Collections.Generic;

namespace BookingSite.ViewModels
{
    public class TicketCalculationRequest
    {
        public int FlightId { get; set; }
        public List<TicketSelectionData> Tickets { get; set; } = new List<TicketSelectionData>();
    }

    public class TicketCalculationResponse
    {
        public double BasePrice { get; set; }
        public int? SelectedMeal { get; set; }
        public int SelectedClass { get; set; }
        public List<int> AvailableClasses { get; set; } = new List<int>();
        public List<PriceFee> Fees { get; set; } = new List<PriceFee>();
    }
}
