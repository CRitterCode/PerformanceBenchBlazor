using Bachelorarbeit_Blazor_Wasm.Pages;
using Bachelorarbeit_Blazor_Wasm.Services;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;

namespace BenchmarkSuite
{
    public class PageBenchmark
    {
        private Version0 _component;

        [GlobalSetup]
        public void Setup()
        {
            _component = new Version0
            {
                Orders = DataFaker.CreateFakeOrders(1000),
                Data = []
            };
            
        }

        [Benchmark]
        public void BenchmarkPopulateChart()
        {
            _component.();
        }
    }
}
