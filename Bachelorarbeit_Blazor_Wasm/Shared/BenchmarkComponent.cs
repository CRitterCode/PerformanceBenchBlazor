
using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class BenchmarkComponent : ComponentBase
    {

        [Inject]
        public BenchmarkUtil BenchmarkUtil { get; set; }


        [Inject]
        public IConfiguration Config { get; set; }



        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter.counter == 0)
            {
                BenchmarkUtil.SetMarker(this, "SetParam");
            }
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter.counter == 0)
            {
                BenchmarkUtil.SetMarker(this, "SetParam_OnInit");
            }
        }

        protected override void OnParametersSet()
        {
            if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter.counter == 0)
            {
                BenchmarkUtil.SetMarker(this, "OnInit_OnParam");
            }
            BenchmarkUtil.ChildStartToFinCounter.counter++;

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                BenchmarkUtil.ChildStartToFinCounter.counter++;
                if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter.counter == BenchmarkUtil.ChildStartToFinCounter.countOrder * 2)
                {
                    BenchmarkUtil.SetMarker(this, "OnParam_OnAfterRender");
                    BenchmarkUtil.SetMarker(this, "FINISH");
                    BenchmarkUtil.ChildStartToFinCounter.counter = 0;
                    BenchmarkUtil.Stopwatch.Reset();
                }
            }
        }

        protected override bool ShouldRender() => false;

        public override string ToString()
        {
            return nameof(BenchmarkComponent);
        }
    }
}

