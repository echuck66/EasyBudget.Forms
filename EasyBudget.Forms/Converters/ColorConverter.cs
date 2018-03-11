using System;
using System.Globalization;
using Xamarin.Forms;

namespace EasyBudget.Forms.Converters
{
    public class ColorConverter : IValueConverter
    {
        public ColorConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorcode = (string)value;
            if (string.IsNullOrEmpty(colorcode))
                colorcode = "#ffffff";

            return Color.FromHex(colorcode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
