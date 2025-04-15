using Microsoft.AspNetCore.Identity;

namespace BookingSite.Data;

public class ApplicationUser: IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastnNme { get; set; }
}
