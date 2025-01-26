using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class VersionComponent : BenchmarkComponent
    {
        public List<Order> Orders { get; set; } = new();
        public List<(string, double)> Data = new();
        public static bool HasLoaded { get; set; } = false;

        [Inject]
        public IJSRuntime JS { get; set; }

        public List<ChartSeries> Series = new List<ChartSeries>();

        public string[] XAxisLabels;

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

            Data.Add(($"Unfinished orders ({Orders.Count - finishedOrders})", Orders.Count - finishedOrders));
            Data.Add(($"Finished orders ({finishedOrders})", finishedOrders));

            var topCategories = Orders.SelectMany(o => o.OrderItems)
           .GroupBy(item => item.Item1.Category)
           .Select(g => new { Category = g.Key, Count = g.Count() })
           .OrderByDescending(x => x.Count)
           .Take(5)
           .ToList();

            Series = new List<ChartSeries>
        {
            new ChartSeries()
            {
                Name = "Categories",
                Data = topCategories.Select(c => (double)c.Count).ToArray()
            }
        };

            XAxisLabels = topCategories.Select(c => c.Category).ToArray();

        }



        protected async Task DownloadBenchmark()
        {
            if (Config.GetValue<bool>("SaveBenchmark"))
            {
                await BenchmarkUtil.DownloadFileAsync(JS, this.GetType().Name);
                BenchmarkUtil.ResetWatch();
                await JS.InvokeVoidAsync("location.reload");
            }
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            BenchmarkUtil.StartWatch();
            return base.SetParametersAsync(parameters);
        }

        //falls reflection wieder nötig...
        //BenchmarkUtil.InvokeWithBenchmarkReflection(this, nameof(this.PopulateChartOrderState));
        protected override void OnInitialized()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker(this, "SetParam_OnInit");
                BenchmarkUtil.InvokeWithBenchmark(this, _ => GenerateOrders(1000), nameof(GenerateOrders), 1);
                BenchmarkUtil.InvokeWithBenchmark(this, _ => PopulateChartOrderState(), nameof(PopulateChartOrderState));

                BenchmarkUtil.SetMarker(this, "OnInit_return");
            }
            else
            {
                GenerateOrders(10);
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
            base.OnParametersSet();
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
            }
            BenchmarkUtil.SetMarker(this, "FINISH");
            base.OnAfterRender(firstRender);
        }

        public override string ToString()
        {
            return nameof(VersionComponent);
        }
    }
}
