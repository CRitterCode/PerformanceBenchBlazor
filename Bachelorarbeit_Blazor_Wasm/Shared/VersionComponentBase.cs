using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class VersionComponentBase : ComponentBase
    {
        public List<Order> Orders { get; set; } = new();
        public List<(string, double)> Data = new();

        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");
        [Inject]
        protected BenchmarkUtil BenchmarkUtil { get; set; }

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

        public override Task SetParametersAsync(ParameterView parameters)
        {
            BenchmarkUtil.StartWatch();
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker("SetParam_OnInit");
                BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.GenerateOrders), 10);
                BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.PopulateChartOrderState));
                BenchmarkUtil.SetMarker("OnInit");
            }
            else
            {
                GenerateOrders(1000);
                PopulateChartOrderState();
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker("OnInit_OnParameter");
            }
            base.OnParametersSet();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (IsBenchmark)
                {
                    BenchmarkUtil.SetMarker("OnInit_OnAfterRender");
                    BenchmarkUtil.InvokeWithBenchmark(this, nameof(this.VisualizeOrderStatusSuccess));
                    BenchmarkUtil.ResetWatch();

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
    }
}
