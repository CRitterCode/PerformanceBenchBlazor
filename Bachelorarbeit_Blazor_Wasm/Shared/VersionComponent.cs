using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class VersionComponent : BenchmarkComponent
    {
        public List<Order> Orders { get; set; } = new();
        public List<(string, double)> Data = new();

        [Inject]
        public IJSRuntime JS { get; set; }

        protected void GenerateOrders(int countOrders)
        {
            Orders = DataFaker.CreateFakeOrders(countOrders);
        }

        protected virtual void VisualizeOrderStatusSuccess()
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
                BenchmarkUtil.SetMarker(this, "Parent_SetParam_OnInit");
                BenchmarkUtil.InvokeWithBenchmark(this, _ => GenerateOrders(1000), nameof(GenerateOrders), 1);
                BenchmarkUtil.InvokeWithBenchmark(this, _ => PopulateChartOrderState(), nameof(PopulateChartOrderState));

                BenchmarkUtil.SetMarker(this, "Parent_OnInit_return");
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


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (IsBenchmark)
                {
                    BenchmarkUtil.SetMarker(this, "OnParam_OnAfterRender");
                    BenchmarkUtil.InvokeWithBenchmark(this, _ => VisualizeOrderStatusSuccess(), nameof(VisualizeOrderStatusSuccess));
                    //BenchmarkUtil.ResetWatch();

                    if (Config.GetValue<bool>("SaveBenchmark"))
                    {
                        await BenchmarkUtil.DownloadFileAsync(JS, this.GetType().Name);
                    }
                }
                else
                {
                    VisualizeOrderStatusSuccess();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public override string ToString()
        {
            return nameof(VersionComponent);
        }
    }
}
