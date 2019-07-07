using System;
using System.Drawing;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;

namespace Chai.WorkflowManagment.CoreDomain.Util
{
    public class ChartBuilder
    {
        protected Chart _chart;
        protected int _numberOfSeries;

        public ChartBuilder(Chart chart, int numberOfSeries)
        {
            _chart = chart;
            _numberOfSeries = numberOfSeries;
        }

        public void BuildChart()
        {
            _chart.ChartAreas.Add(BuildChartArea());
            _chart.Titles.Add(BuildChartTitle());
            if (_numberOfSeries > 1)
                _chart.Legends.Add(BuildChartLegend());

            foreach (Series series in BuildChartSeries())
            {
                _chart.Series.Add(series);
            }
        }

        public Chart TheChart
        {
            get { return _chart; }
        }
        
        protected virtual void CustomizeChartArea(ChartArea area)
        {
        }

        protected virtual void CustomizeChartLegend(Legend legend)
        {

        }

        protected virtual void CustomizeChartSeries(IList<Series> seriesList)
        {

        }

        protected virtual void CustomizeChartTitle(Title title)
        {

        }

        private Legend BuildChartLegend()
        {
            Legend legend = new Legend() { Alignment = StringAlignment.Near, Docking = Docking.Right };
            CustomizeChartLegend(legend);
            return legend;
        }

        private Title BuildChartTitle()
        {
            Title title = new Title() { Docking = Docking.Top, Font = new Font("Trebuchet MS", 18.0f) };
            CustomizeChartTitle(title);
            return title;
        }

        private IList<Series> BuildChartSeries()
        {
            IList<Series> seriesList = new List<Series>();
            for (int i = 0; i < _numberOfSeries - 1; i++)
            {
                Series series = new Series() { ChartType = SeriesChartType.Line, Palette = ChartColorPalette.Pastel, MarkerSize = 10 };
                seriesList.Add(series);
            }
            CustomizeChartSeries(seriesList);
            return seriesList;
        }

        private ChartArea BuildChartArea()
        {
            ChartArea area = new ChartArea() { BackColor = Color.BlanchedAlmond, BackSecondaryColor = Color.Black, BackGradientStyle = GradientStyle.TopBottom };
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.LightStyle = LightStyle.Realistic;
            CustomizeChartArea(area);
            return area;
        }

    }
}
