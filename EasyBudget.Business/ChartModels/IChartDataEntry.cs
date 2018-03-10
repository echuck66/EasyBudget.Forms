using System;
namespace EasyBudget.Business.ChartModels
{
    public interface IChartDataEntry
    {
        float FltValue { get; set; }

        string Label { get; set; }

        string ValueLabel { get; set; }


    }
}
