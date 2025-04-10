using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class Flight
{
    public int Id { get; set; }

    public int? RouteId { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Route? Route { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
