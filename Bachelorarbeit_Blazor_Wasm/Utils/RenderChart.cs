using MudBlazor;

namespace Bachelorarbeit_Blazor_Wasm.Utils
{
    public class RenderChart
    {
        public List<ChartSeries> LineSeries = new();

        public string[] LineXAxisLabels = [];

        public List<(string, double)> PieData = new();

        public void AddPiedData((string label, double value) data)
        {
            PieData.Add(data);
        }

        public void AddLineData((string name, double[] data) seriesData, string[] XAxisLabels)
        {
            LineSeries.Add(new ChartSeries()
            {
                Name = seriesData.name,
                Data = seriesData.data
            });        

            this.LineXAxisLabels = XAxisLabels;

        }
    }
}
