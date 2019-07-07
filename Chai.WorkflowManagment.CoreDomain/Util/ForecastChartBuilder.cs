using System;
using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;

namespace Chai.WorkflowManagment.CoreDomain.Util
{
    public class ForecastChartBuilder : ChartBuilder
    {
        public ForecastChartBuilder(Chart chart, int noSeries) : base(chart ,noSeries)
        {

        }

        protected override void CustomizeChartSeries(IList<Series> seriesList)
        {
            seriesList[0].ChartType = SeriesChartType.Range;
            seriesList[0].Name = "Range";
            seriesList[0].XValueType = ChartValueType.Date;

            seriesList[1].ChartType = SeriesChartType.Line;
            seriesList[1].Name = "Forecasting";
            seriesList[1].XValueType = ChartValueType.Date;

            seriesList[2].ChartType = SeriesChartType.Line;
            seriesList[2].Name = "Input";
            seriesList[2].XValueType = ChartValueType.Date;

            //mySeries.Points.AddXY("ProductName", 1);
        }
        
        public void ClearSeriesPoints()
        {
            _chart.Series[0].Points.Clear();
            _chart.Series[1].Points.Clear();
            _chart.Series[2].Points.Clear();
        }
    }
}
