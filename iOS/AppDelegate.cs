using System;
using System.Collections.Generic;
using System.Linq;
using EasyBudget.Business;
using EasyBudget.Forms;
using Foundation;
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

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
