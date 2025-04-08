namespace PerformanceBenchBlazor.Models
{
    public class Customer
    {
        public Guid CustomerGuid { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
    }
}
