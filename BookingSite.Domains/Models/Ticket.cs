using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? FlightId { get; set; }

    public int? MealId { get; set; }

    public int? SeatNumber { get; set; }

    public SeatClass SeatClass { get; set; } = SeatClass.SecondClass;

    public int? BookingId { get; set; }

    public bool IsCancelled { get; set; } = false;

    public double? Price { get; set; }

    public virtual Flight? Flight { get; set; }

    public virtual MealChoice? Meal { get; set; }

    public virtual Booking? Booking { get; set; }
}

public enum SeatClass
{
    FirstClass = 1,
    SecondClass = 2
}
