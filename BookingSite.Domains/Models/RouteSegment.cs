using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class RouteSegment
{
    public int Id { get; set; }

    public int? RouteId { get; set; }

    public int? SequenceNumber { get; set; }

    public int? AirportId { get; set; }

    public virtual Airport? Airport { get; set; }

    public virtual Route? Route { get; set; }
}
