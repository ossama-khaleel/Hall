namespace Hall_Boking_System.Models
{
    public class JoinTable
    {
        public Customer customer { get; set; }
        public UserLogin userLogin { get; set; }
        public Role role { get; set; }
    }
}
