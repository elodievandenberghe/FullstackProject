using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Season
{
    public int Id { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? AirportId { get; set; }

    public virtual Airport? Airport { get; set; }
}
