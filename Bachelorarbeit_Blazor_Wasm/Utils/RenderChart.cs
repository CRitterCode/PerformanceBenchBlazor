
using PSC.Blazor.Components.Chartjs.Models.Bar;
using PSC.Blazor.Components.Chartjs.Models.Pie;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class RenderChart
    {

        public PieChartConfig PieChartConfig { get; set; } = new();
        public BarChartConfig BarChartConfig { get; set; } = new();

        private readonly List<string> _colors = new()
        {
            "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF",
            "#FF9F40", "#FF6384", "#C9CBCF", "#4BC0C0", "#FF6384"
        };

        public RenderChart()
        {
            PieChartConfig.Data = new PieData
            {
                Labels = new List<string>(),
                Datasets = new List<PieDataset>
        {
            new PieDataset
            {
                Data = new List<decimal?>(),
                BackgroundColor = new List<string>()
            }
        }
            };

            BarChartConfig.Data = new BarData()
            {
                Labels = new List<string>(),
                Datasets = new List<BarDataset>
                {
                    new BarDataset()
                    {
                        Label = "",
                        Data = new List<decimal?>()
                    }
                }
            };
        }

        public void AddPieData(string label, decimal? value)
        {
            PieChartConfig.Data.Labels.Add(label);
            PieChartConfig.Data.Datasets.FirstOrDefault()?.Data.Add(value);
            PieChartConfig.Data.Datasets.FirstOrDefault()?.BackgroundColor.Add(_colors[new Random().Next(_colors.Count)]);
        }

        public void AddBarData(string label, decimal? value)
        {
            BarChartConfig.Data.Labels.Add(label);
            BarChartConfig.Data.Datasets.FirstOrDefault()?.Data.Add(value);
            BarChartConfig.Data.Datasets.FirstOrDefault()?.BackgroundColor.Add(_colors[new Random().Next(_colors.Count)]);
        }


    }
}
