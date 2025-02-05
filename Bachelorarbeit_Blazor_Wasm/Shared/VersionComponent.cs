using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class VersionComponent : BenchmarkComponent
    {
        public List<Order> Orders { get; set; } = new();
        public RenderChart RenderChart { get; set; } = new();

        [Inject]
        public IJSRuntime JS { get; set; }




        protected virtual void GenerateOrders(int countOrders)
        {
            Orders = DataFaker.CreateFakeOrders(countOrders);
        }

        protected virtual void VisualizeOrderStatusSuccess()
        {
            Orders
                .Where(o => o.OrderStatus == OrderState.Arrived)
                .Select(o => o.OrderGuid)
                .ToList()
                .ForEach(guid =>
                    JS.InvokeVoidAsync("updateCardHeaderColorByGuid", guid, "success")
                        );

        }

        protected virtual void PopulateChartOrderState()
        {
            var finishedOrders = Orders.Where(o => o.OrderStatus.HasFlag(OrderState.Arrived)).Count();
            var unfinishedOrders = Orders.Count - finishedOrders;

            RenderChart.AddPieData($"Unfinished orders ({unfinishedOrders})", unfinishedOrders);
            RenderChart.AddPieData($"Finished orders ({finishedOrders})", finishedOrders);

            var categoryDistribution = Orders
                            .SelectMany(o => o.OrderItems)
                            .GroupBy(item => item.Item1.Category)
                            .Select(g => new { Category = g.Key, Count = g.Count() })
                            .OrderByDescending(x => x.Count)
                            .Take(5)
                            .ToList();

            foreach (var item in categoryDistribution)
            {
                RenderChart.AddBarData(item.Category, item.Count);
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var cOrder = Config.GetValue<int>("CountOrders");
            if (IsBenchmark)
            {
                BenchmarkUtil.InvokeWithBenchmark(this, _ => GenerateOrders(cOrder), nameof(GenerateOrders), 1);
                BenchmarkUtil.InvokeWithBenchmark(this, _ => PopulateChartOrderState(), nameof(PopulateChartOrderState));
            }
            else
            {
                GenerateOrders(cOrder);
                PopulateChartOrderState();
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker(this, "OnInit_OnParam");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (IsBenchmark)
                {
                    BenchmarkUtil.SetMarker(this, "OnParam_OnAfterRender");
                    BenchmarkUtil.InvokeWithBenchmark(this, _ => VisualizeOrderStatusSuccess(), nameof(VisualizeOrderStatusSuccess));
                }
                else
                {
                    VisualizeOrderStatusSuccess();
                }
                BenchmarkUtil.SetMarker(this, "FINISH");
            }
        }

        public override string ToString()
        {
            return nameof(VersionComponent);
        }
    }
}
