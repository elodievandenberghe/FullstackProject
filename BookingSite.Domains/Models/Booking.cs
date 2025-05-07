using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSite.Domains.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
    }
}
