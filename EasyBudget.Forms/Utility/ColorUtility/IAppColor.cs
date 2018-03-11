using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EasyBudget.Forms.Utility.ColorUtility
{
    public interface IAppColor
    {
        string Name { set; get; }

        string FriendlyName { set; get; }

        Color Color {  get; }

        string RgbDisplay {  get; }


    }
}
