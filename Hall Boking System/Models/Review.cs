using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Review
    {
        public decimal Id { get; set; }
        public string Opinion { get; set; }
        public decimal? CustomersId { get; set; }
        public decimal? HallId { get; set; }

        public virtual Customer Customers { get; set; }
        public virtual Hall Hall { get; set; }
    }
}
