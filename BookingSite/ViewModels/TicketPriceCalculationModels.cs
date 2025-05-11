using System.Collections.Generic;

namespace BookingSite.ViewModels
{
    public class TicketPriceCalculationRequest
    {
        public int FlightId { get; set; }
        public List<TicketSelectionData> Tickets { get; set; } = new List<TicketSelectionData>();
    }

    public class TicketSelectionData
    {
        public string? SeatClassId { get; set; }
        public int? MealId { get; set; }
    }

    public class TicketPriceResponse
    {
        public double BasePrice { get; set; }
        public List<PriceFee> Fees { get; set; } = new List<PriceFee>();
    }

    public class PriceFee
    {
        public string Title { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
