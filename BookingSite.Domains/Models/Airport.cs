using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Airport
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CityId { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<MealChoice> MealChoices { get; set; } = new List<MealChoice>();

    public virtual ICollection<Route> RouteFromAirports { get; set; } = new List<Route>();

    public virtual ICollection<RouteSegment> RouteSegments { get; set; } = new List<RouteSegment>();

    public virtual ICollection<Route> RouteToAirports { get; set; } = new List<Route>();

    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();
}
