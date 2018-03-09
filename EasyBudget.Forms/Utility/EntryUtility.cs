using System;
using Microcharts;
using SkiaSharp;

namespace EasyBudget.Forms.Utility
{
    internal static class EntryUtility
    {
        internal static Entry GetEntry(float value, SKColor color)
        {
            return _GetEntry(value, color);
        }

        internal static Entry GetEntry(float value)
        {
            return _GetEntry(value, ChartUtility.Instance.GetColor());
        }

        internal static Entry GetEntry(float value, SKColor color, string label, string valueLabel)
        {
            return _GetEntry(value, color, label, valueLabel);
        }

        internal static Entry GetEntry(float value, string label, string valueLabel)
        {
            return _GetEntry(value, ChartUtility.Instance.GetColor(), label, valueLabel);
        }

        internal static Entry GetEntry(float value, string label)
        {
            return _GetEntry(value, ChartUtility.Instance.GetColor(), label, null);
        }

        internal static Entry GetEntry(float value, SKColor color, string label)
        {
            return _GetEntry(value, color, label, null);
        }

        static Entry _GetEntry(float value, SKColor color, string label = null, string valueLabel = null)
        {
            var _entry = new Entry(value)
            {
                Label = label,
                ValueLabel = valueLabel,
                Color = color
            };
            return _entry;
        }
    }
}
