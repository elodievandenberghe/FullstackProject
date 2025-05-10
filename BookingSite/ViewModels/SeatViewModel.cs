namespace BookingSite.ViewModels;

public class SeatViewModel
{
    public int Id { get; set; }
    public int? SeatNumber { get; set; }
    public SeatClassViewModel SeatClass { get; set; }
    public bool IsAvailable { get; set; } = true;

    public string FormattedSeatInfo => $"{SeatClass?.Name} - Seat {SeatNumber}";
}

public class SeatClassViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Available { get; set; } = true;
}
