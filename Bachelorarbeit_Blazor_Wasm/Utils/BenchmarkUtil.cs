using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        public List<string> InvokeWithBenchmark(object objInstance, string methodName, params object[] param)
        {
            if (objInstance != null)
            {
                var methodInfo = objInstance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (methodInfo != null)
                {

                    for (int i = 0; i < Repeat; i++)
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        methodInfo.Invoke(objInstance, param);

                        stopwatch.Stop();

                        Results.Add($"{Identifier},{methodName},{stopwatch.ElapsedMilliseconds}\n"); //{objInstance.GetType().Name}
                        Console.WriteLine($"{methodName} - {stopwatch.ElapsedMilliseconds} ms");
                    }
                }
                else
                {
                    Console.WriteLine($"Methode nicht gefunden - {methodName}");
                }
            }
            return Results;
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
        }

        public async ValueTask<object> DownloadFileAsync(IJSRuntime js, string fileName = "")
        {
            return await js.InvokeAsync<object>("SaveAsFile", fileName + "_" + Identifier, Results);
        }
    }
}
