using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EasyBudget.Forms.Utility.ColorUtility
{
    public class AppColor
    {
        public string Name { set; get; }

        public string FriendlyName { set; get; }

        public Color Color { private set; get; }

        public string RgbDisplay { private set; get; }

        public ColorUtility.GetColorSetDelegate GetAccentColors { get; set; }

        public AppColor(Color color, string name, string hexcode)
        {
            
            this.Color = color;
            this.RgbDisplay = string.Format("{0:X2}-{1:X2}-{2:X2}",
                                           (int)(255 * color.R),
                                            (int)(255 * color.G),
                                            (int)(255 * color.B));
            this.Name = hexcode;
            this.FriendlyName = name;

        }

        public AppColor(Color color, string name, string hexcode, ColorUtility.GetColorSetDelegate getColorSet)
        {
            this.Color = color;
            this.RgbDisplay = string.Format("{0:X2}-{1:X2}-{2:X2}",
                                           (int)(255 * color.R),
                                            (int)(255 * color.G),
                                            (int)(255 * color.B));
            this.Name = hexcode;
            this.FriendlyName = name;
            GetAccentColors = getColorSet;
        }

        static AppColor()
        {
            PrimaryColors = LoadPrimaryColorSet();
        }

        public static IList<AppColor> AppColors { private set; get; }
        public static IList<AppColor> PrimaryColors { private set; get;  }

        public static IList<AppColor> LoadPrimaryColorSet() {
            return ColorUtility.GetPrimaryColors();
        }

        public static IList<AppColor> LoadAccentColors(AppColor primaryColor)
        {
            return primaryColor.GetAccentColors() ?? ColorUtility.GetPrimaryColors();
        }
    }
}
