using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public interface IChartDataGroup
    {
        string Title { get; set; }

        ICollection<ChartData> ChartDataItems { get; set; }
    }
}
