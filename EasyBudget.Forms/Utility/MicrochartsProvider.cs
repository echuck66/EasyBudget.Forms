using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyBudget.Business.ChartModels;
using EasyBudget.Business.ViewModels;
using Microcharts;
using SkiaSharp;
using Entry = Microcharts.Entry;

namespace EasyBudget.Forms.Utility
{
    public class MicrochartsProvider<T> where T : BaseViewModel
    {
        public MicrochartsProvider()
        {
            
        }

        public Chart GetChart(T viewModel, int chartIndex, bool fullSize = false)
        {
            Chart _chart = null;
            List<Entry> entries = new List<Entry>();
            List<ChartDataGroup> _chartGroups = new List<ChartDataGroup>();
            foreach(var _group in viewModel.ChartDataPack.Charts)
            {
                _chartGroups.Add(_group);
            }
            ChartDataGroup _selectedGroup = _chartGroups[chartIndex];
            switch (_selectedGroup.ChartDisplayType)
            {
                case ChartType.Bar:
                    _chart = new BarChart();
                    break;
                case ChartType.Line:
                    _chart = new LineChart();
                    break;
                case ChartType.Pie:
                    _chart = new DonutChart();
                    break;
            }
            foreach(var item in _selectedGroup.ChartDataItems)
            {
                // For now, we'll use a random color
                SKColor color = ChartUtility.Instance.GetColor();

                if (fullSize)
                {
                    entries.Add(EntryUtility.GetEntry(item.FltValue, color, item.Label, item.ValueLabel));
                }
                else
                {
                    entries.Add(EntryUtility.GetEntry(item.FltValue, color));
                }
            }
            _chart.Entries = entries.ToArray();
            return _chart;
        }

        public async Task<Chart> GetChartAsync(T viewModel, int chartIndex, bool fullSize = false)
        {
            Chart _chart = null;
            List<Entry> entries = new List<Entry>();
            List<ChartDataGroup> _chartGroups = new List<ChartDataGroup>();
            foreach (var _group in viewModel.ChartDataPack.Charts)
            {
                _chartGroups.Add(_group);
            }
            ChartDataGroup _selectedGroup = _chartGroups[chartIndex];
            switch (_selectedGroup.ChartDisplayType)
            {
                case ChartType.Bar:
                    _chart = new BarChart();
                    break;
                case ChartType.Line:
                    _chart = new LineChart();
                    break;
                case ChartType.Pie:
                    _chart = new DonutChart();
                    break;
            }
            foreach (var item in _selectedGroup.ChartDataItems)
            {
                SKColor color = !string.IsNullOrEmpty(item.ColorCode) ? SKColor.Parse(item.ColorCode) : ChartUtility.Instance.GetColor();
                if (fullSize)
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry(item.FltValue, color, item.Label, item.ValueLabel)));
                }
                else
                {
                    await Task.Run(() => entries.Add(EntryUtility.GetEntry(item.FltValue, color)));
                }
            }
            _chart.Entries = entries.ToArray();
            return _chart;
        }
    }
}
