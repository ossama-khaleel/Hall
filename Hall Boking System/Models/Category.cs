using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Category
    {
        public Category()
        {
            Halls = new HashSet<Hall>();
        }

        public decimal Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual ICollection<Hall> Halls { get; set; }
    }
}
