using System;
using System.Collections.Generic;

namespace EasyBudget.Business
{
    public class ChartDataGroup
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
