using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class VersionComponentBase : ComponentBase
    {
        public List<Order> Orders { get; set; } = new();
        public List<(string, double)> Data = new();

        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");
        [Inject]
        private BenchmarkUtil BenchmarkUtil { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IConfiguration Config { get; set; }


        protected void GenerateOrders(int countOrders)
        {
            Orders = DataFaker.CreateFakeOrders(countOrders);
        }

        protected void VisualizeOrderStatusSuccess()
        {
            foreach (var order in Orders)
            {
                if (order.OrderStatus == OrderState.Arrived)
                {
                    JS.InvokeVoidAsync("updateCardHeaderColorByGuid", order.OrderGuid, "success");
                }
            }
        }

        protected void PopulateChartOrderState()
        {
            var finishedOrders = Orders.Where(o => o.OrderStatus.HasFlag(OrderState.Arrived)).Count();

            Data.Add(($"Unfinished orders ({Orders.Count - finishedOrders})", Orders.Count - finishedOrders));
            Data.Add(($"Finished orders ({finishedOrders})", finishedOrders));
        }

        protected void PopulateChartSumRevenueByYear()
        {

        }

        protected override Task OnInitializedAsync()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.GenerateOrders), 100);
                BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.PopulateChartOrderState));
            }
            else
            {
                GenerateOrders(10);
                PopulateChartOrderState();
            }

            return base.OnInitializedAsync();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.VisualizeOrderStatusSuccess));

                if (Config.GetValue<bool>("SaveBenchmark"))
                {
                    await BenchmarkUtil.DownloadFileAsync(JS, this.GetType().Name);
                }
            }
            else
            {
                VisualizeOrderStatusSuccess();
            }
            await base.OnAfterRenderAsync(firstRender);
        }


    }
}
