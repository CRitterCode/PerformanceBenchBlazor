
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
            if (parameters.Equals(ParameterView.Empty) is not true)
            {
                SetInitialChildLifecycleMarker("SetParam");
            }
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            SetInitialChildLifecycleMarker("SetParam_OnInit");
        }

        protected override void OnParametersSet()
        {
            SetInitialChildLifecycleMarker("OnInit_OnParam");
            BenchmarkUtil.ChildStartToFinCounter++;

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                BenchmarkUtil.ChildStartToFinCounter--;
                if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter == 0)
                {
                    BenchmarkUtil.SetMarker(this, "OnParam_OnAfterRender");
                    BenchmarkUtil.SetMarker(this, "FINISH");
                    BenchmarkUtil.ResetBenchmark();
                }
            }
        }

        protected override bool ShouldRender() => true;

        public void SetInitialChildLifecycleMarker(string marker)
        {
            if (BenchmarkUtil.IsBenchmark && BenchmarkUtil.ChildStartToFinCounter == 0)
            {
                BenchmarkUtil.SetMarker(this, marker);
            }
        }

        public override string ToString()
        {
            return nameof(BenchmarkComponent);
        }   
    }
}

