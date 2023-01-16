using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Acceptedlist
    {
        public Acceptedlist()
        {
            Acceptances = new HashSet<Acceptance>();
            Testimonials = new HashSet<Testimonial>();
        }

        public decimal Id { get; set; }
        public string AcceptStatus { get; set; }

        public virtual ICollection<Acceptance> Acceptances { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
    }
}
