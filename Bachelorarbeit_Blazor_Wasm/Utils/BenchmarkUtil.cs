using Bachelorarbeit_Blazor_Wasm.Shared;
using Microsoft.JSInterop;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class BenchmarkUtil
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();
        public StringBuilder SbResult { get; set; } = new("ObjInstance,Identifier,MethodName,ElapsedMilliseconds\n");
        private int _repeat => Config.GetValue<int>("RepeatBenchmark");
        private Stopwatch _stopwatch { get; set; } = new();

        public IConfiguration Config { get; set; }

        public BenchmarkUtil(IConfiguration config)
        {
            Config = config;
        }

        public void InvokeWithBenchmark(BenchmarkComponent component, Action<BenchmarkComponent> fn, string nameOfMethod, int? Repeat = null)
        {
            Repeat = Repeat ?? this._repeat;

            for (int i = 0; i < Repeat; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                fn.Invoke(component);

                stopwatch.Stop();

                SbResult.AppendLine($"{component},{Identifier},{nameOfMethod},{stopwatch.ElapsedMilliseconds}");
                Console.WriteLine($"{component} - {nameOfMethod} - {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public void StartWatch()
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }
        }

        public void ResetWatch()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Reset();
            }
        }

        public void SetMarker(object component, string marker)
        {
            if (_stopwatch.IsRunning)
            {
                SbResult.AppendLine($"{component},{Identifier},{marker},{_stopwatch.ElapsedMilliseconds}");
                Console.WriteLine($"{component} - {marker} - {_stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                Console.WriteLine($"Stopwatch not running, can't set marker");
            }
        }

        public async ValueTask<object> DownloadFileAsync(IJSRuntime js, string fileName = "")
        {
            var cOrders = Config.GetValue<int>("CountOrders");
            var isAOT = Config.GetValue<bool>("AOT");
            return await js.InvokeAsync<object>("SaveAsFile", fileName + "_" + Identifier + "_" + cOrders + "_" + _repeat + "_" + isAOT, SbResult.ToString());
        }
    }
}
