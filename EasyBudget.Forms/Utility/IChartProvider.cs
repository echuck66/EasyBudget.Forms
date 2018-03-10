using System;
using System.Threading.Tasks;
using EasyBudget.Business.ViewModels;
using Microcharts;
using Entry = Microcharts.Entry;

namespace EasyBudget.Forms.Utility
{
    public interface IChartProvider<T> where T : BaseViewModel
    {
        object GetChart(T viewModel, int chartIndex, bool fullSize);

        Task<object> GetChartAsync(T viewModel, int chartIndex, bool fullSize = false);
    }
}
