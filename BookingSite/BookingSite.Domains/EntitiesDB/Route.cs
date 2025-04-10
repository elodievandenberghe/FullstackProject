using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class Route
{
    public int Id { get; set; }

    public int? FromAirportId { get; set; }

    public int? ToAirportId { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual Airport? FromAirport { get; set; }

    public virtual ICollection<RouteSegment> RouteSegments { get; set; } = new List<RouteSegment>();

    public virtual Airport? ToAirport { get; set; }
}
