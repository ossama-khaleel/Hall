using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Acceptances = new HashSet<Acceptance>();
            Payments = new HashSet<Payment>();
            Reservations = new HashSet<Reservation>();
            Testimonials = new HashSet<Testimonial>();
            UserLogins = new HashSet<UserLogin>();
            Visas = new HashSet<Visa>();
            Reviews = new HashSet<Review>();
        }

        public decimal Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserImage { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual ICollection<Acceptance> Acceptances { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<Visa> Visas { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
