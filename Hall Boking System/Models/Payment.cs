using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Reservations = new HashSet<Reservation>();
        }

        public decimal Id { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal? CustomersId { get; set; }
        public decimal? VisaId { get; set; }

        public virtual Customer Customers { get; set; }
        public virtual Visa Visa { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
