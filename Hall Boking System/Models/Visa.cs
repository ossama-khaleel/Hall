using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Visa
    {
        public Visa()
        {
            Payments = new HashSet<Payment>();
        }

        public decimal Id { get; set; }
        public string VisaName { get; set; }
        public string? VisaNumber { get; set; }
        public decimal VisaAmount { get; set; }
        public decimal? CustomersId { get; set; }

        public virtual Customer Customers { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
