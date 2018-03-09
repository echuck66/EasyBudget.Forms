using System;
namespace EasyBudget.Business.ChartModels
{
    public class ChartData : IChartData
    {
        public float FltValue { get; set; }

        public string Label { get; set; }

        public string ValueLabel { get; set; }

        public ChartData()
        {
            
        }

        public ChartData(float fltValue)
        {
            FltValue = fltValue;
        }

        public ChartData(float value, string label = null, string valueLabel = null)
        {
            FltValue = value;
            Label = label;
            ValueLabel = valueLabel;
        }

        public ChartData(DateTime value, string label = null, string valueLabel = null)
        {
            FltValue = (float)value.ToOADate();
            Label = label;
            ValueLabel = valueLabel;
        }
    }
}
