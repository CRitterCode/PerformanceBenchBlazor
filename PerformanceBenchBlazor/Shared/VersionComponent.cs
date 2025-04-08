using PerformanceBenchBlazor.Models;
using PerformanceBenchBlazor.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PerformanceBenchBlazor.Shared
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

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            var cOrder = BenchmarkUtil.Config.GetValue<int>("CountOrders");
            if (BenchmarkUtil.IsBenchmark )
            {
                BenchmarkUtil.SetMarker(this, "SetParam_OnInit");
                BenchmarkUtil.InvokeWithBenchmark(this, _ => GenerateOrders(cOrder), nameof(GenerateOrders), 1);
                BenchmarkUtil.InvokeWithBenchmark(this, _ => PopulateChartOrderState(), nameof(PopulateChartOrderState));
            }
            else
            {
                GenerateOrders(cOrder);
                PopulateChartOrderState();
            }
        }

        protected override void OnParametersSet()
        {
            if (BenchmarkUtil.IsBenchmark)
            {
                BenchmarkUtil.SetMarker(this, "OnInit_OnParam");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (BenchmarkUtil.IsBenchmark)
                {
                    BenchmarkUtil.SetMarker(this, "OnParam_OnAfterRender");
                    BenchmarkUtil.InvokeWithBenchmark(this, _ => VisualizeOrderStatusSuccess(), nameof(VisualizeOrderStatusSuccess));
                    BenchmarkUtil.SetMarker(this, "FINISH");
                }
                else
                {
                    VisualizeOrderStatusSuccess();
                }
            }
        }


        protected override bool ShouldRender() => false;

        public override string ToString()
        {
            return nameof(VersionComponent);
        }
    }
}
