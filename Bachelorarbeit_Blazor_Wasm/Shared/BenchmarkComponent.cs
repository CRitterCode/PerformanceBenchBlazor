
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

        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");
        protected bool HasFirstSet { get; set; } = false;

        [Inject]
        public BenchmarkUtil BenchmarkUtil { get; set; }


        [Inject]
        public IConfiguration Config { get; set; }



        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.StartWatch();
            }
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            if (IsBenchmark && (HasFirstSet || this is not VersionComponent))
            {
                BenchmarkUtil.SetMarker(this, "SetParam_OnInit");
            }
            HasFirstSet = true;
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
                }
                BenchmarkUtil.SetMarker(this, "FINISH");
            }
        }

        protected override bool ShouldRender() => false;

        public override string ToString()
        {
            return nameof(BenchmarkComponent);
        }
    }
}

