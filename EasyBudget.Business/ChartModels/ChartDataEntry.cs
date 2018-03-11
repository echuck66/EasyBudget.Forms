using System;

namespace EasyBudget.Business.ChartModels
{
    public class ChartDataEntry : IChartDataEntry
    {
        public float FltValue { get; set; }

        public string Label { get; set; }

        public string ValueLabel { get; set; }

        public string ColorCode { get; set; }

        public ChartDataEntry()
        {
            
        }

        public ChartDataEntry(float fltValue)
        {
            FltValue = fltValue;
        }

        public ChartDataEntry(float value, string colorCode, string label = null, string valueLabel = null)
        {
            FltValue = value;
            Label = label;
            ValueLabel = valueLabel;
            ColorCode = colorCode;
        }

        public ChartDataEntry(DateTime value, string colorCode, string label = null, string valueLabel = null)
        {
            FltValue = (float)value.ToOADate();
            Label = label;
            ValueLabel = valueLabel;
            ColorCode = colorCode;
        }
    }

}
