using System;
using System.Collections.Generic;

#nullable disable

namespace Hall_Boking_System.Models
{
    public partial class Role
    {
        public Role()
        {
            UserLogins = new HashSet<UserLogin>();
        }

        public decimal Id { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserLogin> UserLogins { get; set; }
    }
}
