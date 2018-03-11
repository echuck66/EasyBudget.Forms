using System;
using System.Globalization;
using EasyBudget.Forms.Utility.ColorUtility;
using Xamarin.Forms;

namespace EasyBudget.Forms.Converters
{
    public class ColorNameConverter : IValueConverter
    {
        public ColorNameConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorcode = (string)value;
            if (string.IsNullOrEmpty(colorcode))
                colorcode = "#ffffff";

            AppColor appColor = ColorUtility.FindAppColor(colorcode);

            return appColor.FriendlyName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
