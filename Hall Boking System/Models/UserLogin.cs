using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class UserLogin
    {
        public decimal Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal? RoleId { get; set; }
        public decimal? CustomersId { get; set; }
        [NotMapped]
        public virtual IFormFile ImageFile { get; set; }
        public virtual Customer Customers { get; set; }
        public virtual Role Role { get; set; }
    }
}
