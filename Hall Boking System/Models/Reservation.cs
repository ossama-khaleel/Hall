using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            Acceptances = new HashSet<Acceptance>();
        }

        public decimal Id { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public decimal? HallId { get; set; }
        public decimal? CustomersId { get; set; }
        public decimal? PaymentId { get; set; }
        [NotMapped]
        public decimal? VisaId { get; set; }
        public virtual Customer Customers { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<Acceptance> Acceptances { get; set; }
    }
}
