﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EasyBudget.Forms;
using EasyBudget.Business;
using Xamarin.Forms.Platform.UWP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EasyBudget.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage //: Page
    {
        DIContainer container = new DIContainer();

        public MainPage()
        {
            
            this.InitializeComponent();
            container.Register<IDataServiceHelper, DataServiceHelper>();
            container.Create<EasyBudgetDataService>();

            LoadApplication(new EasyBudget.Forms.App());
        }
    }
}
