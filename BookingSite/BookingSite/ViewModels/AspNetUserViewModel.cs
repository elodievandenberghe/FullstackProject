namespace BookingSite.ViewModels;

public class AspNetUserViewModel
{
    public string Id { get; set; } = null!;
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public string? Email { get; set; }
    
    public bool EmailConfirmed { get; set; }
    
}
