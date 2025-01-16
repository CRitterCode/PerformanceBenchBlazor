namespace Bachelorarbeit_Blazor_Wasm.Entities
{
    public class StockItem
    {
        public Guid OrderGuid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.MinValue;

    }
}
