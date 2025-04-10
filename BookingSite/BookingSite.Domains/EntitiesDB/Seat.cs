using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class Seat
{
    public int Id { get; set; }

    public int? TravelClassId { get; set; }

    public string? SeatNumber { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual TravelClass? TravelClass { get; set; }
}
