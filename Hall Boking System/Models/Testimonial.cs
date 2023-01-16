using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Testimonial
    {
        public decimal Id { get; set; }
        public string TestimonialOpinion { get; set; }
        public decimal CustomersId { get; set; }
        public decimal? AcceptlistId { get; set; }
        public decimal? HomeId { get; set; }

        public virtual Acceptedlist Acceptlist { get; set; }
        public virtual Customer Customers { get; set; }
        public virtual Home Home { get; set; }
    }
}
