namespace Bachelorarbeit_Blazor_Wasm.Helpers.Attributes
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BenchmarkStopwatchAttribute : Attribute
    {
        public static void MeasureExecutionTime(object instance, string methodName, int repeat, params object[] parameters)
        {
            var type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes<BenchmarkStopwatchAttribute>();

                if (attributes.Any())
                {
                    for (int i = 0; i < repeat; i++)
                    {
                        var stopwatch = Stopwatch.StartNew();
                        method.Invoke(instance, parameters);
                        stopwatch.Stop();
                        Console.WriteLine($"{method.Name} executed in {stopwatch.ElapsedMilliseconds} ms");
                    }
                }
            }
        }

    }

}
