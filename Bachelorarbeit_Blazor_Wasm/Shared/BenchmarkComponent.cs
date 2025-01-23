
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
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker("Child_SetParam_OnInit");
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker("Child_OnInit_OnParameter");
            }
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (IsBenchmark)
                {
                    BenchmarkUtil.SetMarker("Child_OnParam_OnAfterRender");
                    //BenchmarkUtil.ResetWatch();
                }
            }
            base.OnAfterRender(firstRender);
        }
    }
}

