using System;
using System.Collections.Generic;
using System.Linq;
using EasyBudget.Business;
using EasyBudget.Forms;
using Foundation;
using Google.MobileAds;
using UIKit;

namespace EasyBudget.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        DIContainer container = new DIContainer();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            
            global::Xamarin.Forms.Forms.Init();

            container.Register<IDataServiceHelper, DataServiceHelper>();
            container.Create<EasyBudgetDataService>();

            MobileAds.Configure("ca-app-pub-6358883513529026~4075666294");
            //MobileAds.Configure("ca-app-pub-3940256099942544~3347511713");
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
