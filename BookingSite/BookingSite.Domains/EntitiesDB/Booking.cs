using System;
using System.Collections.Generic;

namespace BookingSite.Domains.EntitiesDB;

public partial class Booking
{
    public int? UserId { get; set; }

    public int? TicketId { get; set; }

    public int PurchaseId { get; set; }

    public virtual Ticket? Ticket { get; set; }

    public virtual User? User { get; set; }
}
