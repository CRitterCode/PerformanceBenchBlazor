namespace Bachelorarbeit_Blazor_Wasm.Helpers.Attributes
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public static class BenchmarkHelper
    {
        public static void InvokeWithBenchmark(object objInstance, string methodName, int repeat = 1, params object[] param)
        {
            if (objInstance != null)
            {
                var methodInfo = objInstance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (methodInfo != null)
                {
                    for (int i = 0; i < repeat; i++)
                    {

                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        methodInfo.Invoke(objInstance, param);

                        stopwatch.Stop();
                        Console.WriteLine($"{methodName} - {stopwatch.ElapsedMilliseconds} ms");
                    }
                }
                else
                {
                    Console.WriteLine($"Methode nicht gefunden - {methodName}");
                }
            }
        }
    }

}
