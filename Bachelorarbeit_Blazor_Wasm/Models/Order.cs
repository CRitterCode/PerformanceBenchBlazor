namespace Bachelorarbeit_Blazor_Wasm.Entities
{
    public class Order
    {
        public Guid OrderGuid { get; init; } = Guid.NewGuid();
        public Customer Customer { get; init; } = new();

        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public OrderState OrderStatus { get; set; } = OrderState.Pending;
        public List<(StockItem Item, int Quantity)> OrderItems { get; set; } = [];
    }
}
