using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Hall
    {
        public Hall()
        {
            Acceptances = new HashSet<Acceptance>();
            Reservations = new HashSet<Reservation>();
            Reviews = new HashSet<Review>();
        }

        public decimal Id { get; set; }
        public string HallName { get; set; }
        public decimal HallPrice { get; set; }
        public string HallDescription { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? AddressId { get; set; }
        public string HallImage { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual Address Address { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Acceptance> Acceptances { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
