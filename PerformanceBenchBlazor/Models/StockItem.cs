namespace PerformanceBenchBlazor.Models
{
    public class StockItem
    {
        public Guid OrderGuid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.MinValue;
        public string ProductMaterial { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

    }
}
