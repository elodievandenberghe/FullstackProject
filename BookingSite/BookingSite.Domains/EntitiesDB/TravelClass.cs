using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class TravelClass
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
