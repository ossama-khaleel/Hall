namespace Hall_Boking_System.Models
{
    public class JoinPayAcc
    {
        public Acceptance acceptance { get; set; }
        public Reservation reservation { get; set; }
        public Payment payment { get; set; }
    }
}
