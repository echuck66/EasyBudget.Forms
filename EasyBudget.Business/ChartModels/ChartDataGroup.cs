using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public class ChartDataGroup : IChartDataGroup
    {
        public string Title { get; set; }

        public ICollection<ChartData> ChartDataItems { get; set; }

        public ChartDataGroup()
        {
            ChartDataItems = new List<ChartData>();
        }

        public ChartDataGroup(string title, ICollection<ChartData> chartData){
            this.Title = title;
            this.ChartDataItems = new List<ChartData>(chartData);
        }
    }
}
