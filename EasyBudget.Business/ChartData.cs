using System;
namespace EasyBudget.Business
{
    public class ChartData
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

        public ChartData(float fltValue, string label = null, string valueLabel = null)
        {
            FltValue = fltValue;
            Label = label;
            ValueLabel = valueLabel;
        }
    }
}
