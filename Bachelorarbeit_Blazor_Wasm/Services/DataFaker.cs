using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Helpers.Attributes;
using Bogus;

namespace Bachelorarbeit_Blazor_Wasm.Services
{
    public static class DataFaker
    {
        public static List<Order> CreateFakeOrders(int numberOfOrders)
        {
            var bogusFaker = new Faker();            
            var orders = new List<Order>();

            for (int cOrder = 0; cOrder < numberOfOrders; cOrder++)
            {
                var customer = new Customer
                {
                    FirstName = bogusFaker.Name.FirstName(),
                    LastName = bogusFaker.Name.LastName(),
                    Address = new Address
                    {
                        City = bogusFaker.Address.City(),
                        Country = bogusFaker.Address.Country(),
                        Street = bogusFaker.Address.StreetName(),
                        StreetNumber = bogusFaker.Random.Number(50),
                        ZipCode = bogusFaker.Address.ZipCode()
                    }
                };
                

                var order = new Order
                {
                    Customer = customer,
                    OrderDate = bogusFaker.Date.Past(1),
                    OrderStatus = bogusFaker.PickRandom<OrderState>()
                };

                int numberOfItems = bogusFaker.Random.Int(1, 10);

                for (int i = 0; i < numberOfItems; i++)
                {
                    var quantity = bogusFaker.Random.Int(1, 10);
                    var stockItem = new StockItem
                    {
                        Name = bogusFaker.Commerce.ProductName(),
                        Price = bogusFaker.Random.Decimal(1, 500)
                    };

                    order.OrderItems.Add((stockItem, quantity));
                }

                orders.Add(order);
            }

            return orders;
        }
    }
}
