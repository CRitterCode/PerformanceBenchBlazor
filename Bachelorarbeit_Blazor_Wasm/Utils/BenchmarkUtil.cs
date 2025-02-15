using Bachelorarbeit_Blazor_Wasm.Shared;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Text;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class BenchmarkUtil
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();
        private StringBuilder _strBuilder { get; set; } = new("ObjInstance,Identifier,MethodName,ElapsedMilliseconds\n");
        private int _repeat => Config.GetValue<int>("RepeatBenchmark");

        public int ChildStartToFinCounter;
        public bool IsBenchmark => Config.GetValue<bool>("IsBenchmark");

        public Stopwatch Stopwatch { get; set; } = new();

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
                _strBuilder.AppendLine($"{component},{Identifier},{nameOfMethod},{stopwatch.ElapsedMilliseconds}");
                Console.WriteLine($"{component} - {nameOfMethod} - {stopwatch.ElapsedMilliseconds} ms");
            }
        }

        public void SetMarker(object component, string marker)
        {
            if (Stopwatch.IsRunning)
            {
                _strBuilder.AppendLine($"{component},{Identifier},{marker},{Stopwatch.ElapsedMilliseconds}");
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
            _strBuilder.Clear();
            _strBuilder.AppendLine("ObjInstance,Identifier,MethodName,ElapsedMilliseconds");
            ChildStartToFinCounter = 0;
        }

        public async ValueTask<object> DownloadFileAsync(IJSRuntime js, string fileName = "")
        {
            var cOrders = Config.GetValue<int>("CountOrders");
            var isAOT = Config.GetValue<bool>("AOT");
            return await js.InvokeAsync<object>("SaveAsFile", fileName + "_" + Identifier + "_" + cOrders + "_" + _repeat + "_" + isAOT, _strBuilder.ToString());
        }
    }
}
