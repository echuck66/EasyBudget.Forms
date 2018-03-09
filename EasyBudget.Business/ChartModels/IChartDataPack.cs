using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public interface IChartDataPack
    {
        ICollection<ChartDataGroup> Charts { get; set; }
    }
}
