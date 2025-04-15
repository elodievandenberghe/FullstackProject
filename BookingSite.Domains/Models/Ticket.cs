using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? FlightId { get; set; }

    public int? MealId { get; set; }

    public int? SeatId { get; set; }

    public bool? IsCancelled { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Flight? Flight { get; set; }

    public virtual MealChoice? Meal { get; set; }

    public virtual Seat? Seat { get; set; }
}
