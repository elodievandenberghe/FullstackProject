namespace BookingSite.ViewModels;

public class MealChoiceViewModel
{
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Description { get; set; }
    public int? AirportId { get; set; }
    public string? AirportName { get; set; }
}
