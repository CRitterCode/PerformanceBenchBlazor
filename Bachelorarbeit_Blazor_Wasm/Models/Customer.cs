namespace Bachelorarbeit_Blazor_Wasm.Entities
{
    public class Customer
    {
        public Guid CustomerGuid { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
    }
}
