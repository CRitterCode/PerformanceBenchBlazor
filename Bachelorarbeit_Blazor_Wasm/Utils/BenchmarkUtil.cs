using Bachelorarbeit_Blazor_Wasm.Shared;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Text;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class BenchmarkUtil
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();
        public StringBuilder SbResult { get; set; } = new("ObjInstance,Identifier,MethodName,ElapsedMilliseconds\n");
        private int _repeat => Config.GetValue<int>("RepeatBenchmark");

        public (int counter, int countOrder) ChildStartToFinCounter;
        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");

        public Stopwatch Stopwatch { get; set; } = new();

        public IConfiguration Config { get; set; }

        public BenchmarkUtil(IConfiguration config)
        {
            Config = config;
            ChildStartToFinCounter = new(0, Config.GetValue<int>("CountOrders"));
        }

        public void InvokeWithBenchmark(VersionComponent component, Action<VersionComponent> fn, string nameOfMethod, int? Repeat = null)
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

        public void SetMarker(object component, string marker)
        {
            if (Stopwatch.IsRunning)
            {
                SbResult.AppendLine($"{component},{Identifier},{marker},{Stopwatch.ElapsedMilliseconds}");
                Console.WriteLine($"{component} - {marker} - {Stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                Console.WriteLine($"Stopwatch not running, can't set marker");
            }
        }

        public void ResetBenchmark()
        {
            Stopwatch.Restart();
            SbResult.Clear();
            SbResult.AppendLine("ObjInstance,Identifier,MethodName,ElapsedMilliseconds");
            ChildStartToFinCounter = (0, ChildStartToFinCounter.countOrder);
        }

        public async ValueTask<object> DownloadFileAsync(IJSRuntime js, string fileName = "")
        {
            var cOrders = Config.GetValue<int>("CountOrders");
            var isAOT = Config.GetValue<bool>("AOT");
            return await js.InvokeAsync<object>("SaveAsFile", fileName + "_" + Identifier + "_" + cOrders + "_" + _repeat + "_" + isAOT, SbResult.ToString());
        }
    }
}
