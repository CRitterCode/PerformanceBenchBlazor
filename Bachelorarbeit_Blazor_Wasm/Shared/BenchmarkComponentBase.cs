
using Bachelorarbeit_Blazor_Wasm.Entities;
using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Bachelorarbeit_Blazor_Wasm.Shared
{
    public class BenchmarkComponentBase : ComponentBase
    {

        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");

        [Inject]
        protected BenchmarkUtil BenchmarkUtil { get; set; }


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
                BenchmarkUtil.SetMarker("#SetParam_OnInit");
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (IsBenchmark)
            {
                BenchmarkUtil.SetMarker("#OnInit_OnParameter");
            }
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (IsBenchmark)
                {
                    BenchmarkUtil.SetMarker("OnInit_OnAfterRender");
                    BenchmarkUtil.ResetWatch();

                    //if (Config.GetValue<bool>("SaveBenchmark"))
                    //{
                    //    await BenchmarkUtil.DownloadFileAsync(JS, this.GetType().Name);
                    //}
                }
            }
            base.OnAfterRender(firstRender);
        }
    }
}

