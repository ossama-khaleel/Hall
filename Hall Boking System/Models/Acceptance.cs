using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Acceptance
    {
        public decimal Id { get; set; }
        public DateTime? AcceptDate { get; set; }
        public decimal AcceptlistId { get; set; }
        public decimal? CustomersId { get; set; }
        public decimal? HallId { get; set; }
        public decimal? ReservationId { get; set; }

        public virtual Acceptedlist Acceptlist { get; set; }
        public virtual Customer Customers { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
