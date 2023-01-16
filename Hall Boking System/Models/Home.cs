using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Home
    {
        public Home()
        {
            Aboutus = new HashSet<Aboutu>();
            Contactus = new HashSet<Contactu>();
            Testimonials = new HashSet<Testimonial>();
        }

        public decimal Id { get; set; }
        public string HomeImage1 { get; set; }
        public string HomeImage2 { get; set; }
        public string HomeImage3 { get; set; }

        [NotMapped]
        public virtual IFormFile ImageFile1 { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile2 { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile3 { get; set; }

        public virtual ICollection<Aboutu> Aboutus { get; set; }
        public virtual ICollection<Contactu> Contactus { get; set; }
        public virtual ICollection<Testimonial> Testimonials { get; set; }
    }
}
