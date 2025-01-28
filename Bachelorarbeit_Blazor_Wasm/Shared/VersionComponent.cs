using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

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

            RenderChart.AddPiedData(($"Unfinished orders ({Orders.Count - finishedOrders})", Orders.Count - finishedOrders));
            RenderChart.AddPiedData(($"Finished orders ({finishedOrders})", finishedOrders));

            var topCategories = Orders.SelectMany(o => o.OrderItems)
           .GroupBy(item => item.Item1.Category)
           .Select(g => new { Category = g.Key, valCount = g.Count() })
           .OrderByDescending(x => x.valCount)
           .Take(5)
           .ToList();

            RenderChart.AddLineData(("Categories", topCategories.Select(c => (double)c.valCount).ToArray()),
                                    topCategories.Select(c => c.Category).ToArray());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var cOrder = Config.GetValue<int>("CountOrders");
            if (IsBenchmark)
            {
                //BenchmarkUtil.SetMarker(this, "SetParam_OnInit");
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
