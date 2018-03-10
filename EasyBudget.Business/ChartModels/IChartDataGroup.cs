using System;
using System.Collections.Generic;

namespace EasyBudget.Business.ChartModels
{
    public interface IChartDataGroup
    {
        string Title { get; set; }

        ICollection<ChartDataEntry> ChartDataItems { get; set; }

        ChartType ChartDisplayType { get; set; }
    }

    public enum ChartType
    {
        Bar,
        Line,
        Pie
    }
}
