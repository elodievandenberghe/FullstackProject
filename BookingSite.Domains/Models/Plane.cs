using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Plane
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
