using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class MealChoice
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public int? AirportId { get; set; }

    public virtual Airport? Airport { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
