using Bachelorarbeit_Blazor_Wasm.Models;
using Bogus;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public static class DataFaker
    {
        public static List<Order> CreateFakeOrders(int numberOfOrders)
        {
            Randomizer.Seed = new Random(8675309);
            var bogusFaker = new Faker();            
            var orders = new List<Order>();

            

            for (int cOrder = 0; cOrder < numberOfOrders; cOrder++)
            {
                var customer = new Customer
                {
                    FirstName = bogusFaker.Name.FirstName(),
                    LastName = bogusFaker.Name.LastName(),
                    PhoneNumber = bogusFaker.Phone.PhoneNumber(),
                    Email = bogusFaker.Internet.ExampleEmail(),
                    IBAN = bogusFaker.Finance.Iban(),

                    Address = new Address
                    {
                        City = bogusFaker.Address.City(),
                        Country = bogusFaker.Address.Country(),
                        Street = bogusFaker.Address.StreetName(),
                        StreetNumber = bogusFaker.Random.Number(50),
                        ZipCode = bogusFaker.Address.ZipCode()
                    }
                };


                var categories = bogusFaker.Commerce.Categories(10);
                

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
                        Price = bogusFaker.Random.Decimal(1, 500),
                        ProductMaterial = bogusFaker.Commerce.ProductMaterial(),
                        Color = bogusFaker.Commerce.Color(),
                        Category = bogusFaker.PickRandomParam(categories)
                        
                    };

                    order.OrderItems.Add((stockItem, quantity));
                }

                orders.Add(order);
            }

            return orders;
        }
    }
}
