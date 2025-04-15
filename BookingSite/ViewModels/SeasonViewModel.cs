namespace BookingSite.ViewModels;

public class SeasonViewModel
{
    public int Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? AirportId { get; set; }
    public string? AirportName { get; set; }
}
