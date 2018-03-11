using System;
using System.Globalization;
using EasyBudget.Forms.Utility.ColorUtility;
using Xamarin.Forms;

namespace EasyBudget.Forms.Converters
{
    public class ColorCodeConverter : IValueConverter
    {
        public ColorCodeConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                value = "#ffffff";

            string colorCode = (string)value;
            AppColor appColor = ColorUtility.FindAppColor(colorCode);

            return appColor.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
