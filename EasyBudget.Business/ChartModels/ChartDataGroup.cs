using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public class ChartDataGroup : IChartDataGroup
    {
        public string Title { get; set; }

        public ICollection<ChartDataEntry> ChartDataItems { get; set; }

        public ChartType ChartDisplayType { get; set; }

        public int ChartDisplayOrder { get; set; }

        public ChartDataGroup()
        {
            ChartDataItems = new List<ChartDataEntry>();
        }

        public ChartDataGroup(string title, ICollection<ChartDataEntry> chartData, ChartType chartDispayType){
            this.Title = title;
            this.ChartDataItems = new List<ChartDataEntry>(chartData);
            this.ChartDisplayType = chartDispayType;
        }
    }
}
