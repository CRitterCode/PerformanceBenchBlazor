namespace Bachelorarbeit_Blazor_Wasm.Models
{
    public class Address
    {
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int StreetNumber { get; set; } = int.MinValue;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
