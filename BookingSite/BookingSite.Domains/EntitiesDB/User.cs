using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class User
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
