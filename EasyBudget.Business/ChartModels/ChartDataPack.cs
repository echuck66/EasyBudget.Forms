using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public class ChartDataPack : IChartDataPack
    {
        public ICollection<ChartDataGroup> Charts { get; set; }

        public ChartDataPack()
        {
            this.Charts = new List<ChartDataGroup>();
        }
    }
}
