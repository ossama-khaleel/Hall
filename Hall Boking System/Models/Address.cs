using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Address
    {
        public Address()
        {
            Halls = new HashSet<Hall>();
        }

        public decimal Id { get; set; }
        public string City { get; set; }

        public virtual ICollection<Hall> Halls { get; set; }
    }
}
