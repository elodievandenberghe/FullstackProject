using System;
using System.Collections.Generic;

namespace BookingSite.Domains.Models;

public partial class Booking
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int TicketId { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
