namespace WebServer.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string InsurancePolicyNumber { get; set; }
    }
}
