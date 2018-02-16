using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace EasyBudget.Forms
{
    public class CurrencyConverter : IValueConverter
    {
        public CurrencyConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
