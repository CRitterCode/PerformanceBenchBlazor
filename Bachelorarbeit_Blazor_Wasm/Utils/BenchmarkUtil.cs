using Bachelorarbeit_Blazor_Wasm.Shared;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Reflection;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class BenchmarkUtil
    {
        public Guid Identifier { get; set; } = Guid.NewGuid();
        public List<string> Results { get; set; } = new() { "ObjInstance,Identifier,MethodName,ElpasedMilliseconds\n" };
        public int Repeat => Config.GetValue<int>("RepeatBenchmark");
        private Stopwatch _stopwatch { get; set; } = new();

        public IConfiguration Config { get; set; }

        public BenchmarkUtil(IConfiguration config)
        {
            Config = config;
        }

        public void InvokeWithBenchmark(BenchmarkComponent component, Action<BenchmarkComponent> fn, string nameOfMethod, int? Repeat = null)
        {
            Repeat = Repeat ?? this.Repeat;
            for (int i = 0; i < Repeat; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                fn.Invoke(component);

                stopwatch.Stop();

                Results.Add($"{Identifier},{nameOfMethod},{stopwatch.ElapsedMilliseconds}\n");
                Console.WriteLine($"{nameOfMethod} - {stopwatch.ElapsedMilliseconds} ms");
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

        public void SetMarker(string marker)
        {
            if (_stopwatch.IsRunning)
            {
                Results.Add($"{Identifier},{marker},{_stopwatch.ElapsedMilliseconds}\n");
                Console.WriteLine($"{marker} - {_stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                Console.WriteLine($"Stopwatch not running, can't set marker");
            }
        }

        public async ValueTask<object> DownloadFileAsync(IJSRuntime js, string fileName = "")
        {
            return await js.InvokeAsync<object>("SaveAsFile", fileName + "_" + Identifier, Results);
        }

        // Geht auch ohne reflection, wenn man nur components verwendet
        //public List<string> InvokeWithBenchmarkReflection(object objInstance, string methodName, params object[] param)
        //{
        //    if (objInstance != null)
        //    {
        //        var methodInfo = objInstance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        //        if (methodInfo != null)
        //        {

        //            for (int i = 0; i < Repeat; i++)
        //            {
        //                var stopwatch = new Stopwatch();
        //                stopwatch.Start();

        //                methodInfo.Invoke(objInstance, param);

        //                stopwatch.Restart();

        //                Results.Add($"{Identifier},{methodName},{stopwatch.ElapsedMilliseconds}\n"); //{objInstance.GetType().Name}
        //                Console.WriteLine($"{methodName} - {stopwatch.ElapsedMilliseconds} ms");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Methode nicht gefunden - {methodName}");
        //        }
        //    }
        //    return Results;
        //}
    }
}
