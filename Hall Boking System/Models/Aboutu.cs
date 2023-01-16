using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Aboutu
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal? HomeId { get; set; }

        public virtual Home Home { get; set; }
    }
}
