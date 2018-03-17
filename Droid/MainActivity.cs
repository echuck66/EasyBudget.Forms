using System;
using EasyBudget.Forms;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using EasyBudget.Business;
using Android.Gms.Ads;

namespace EasyBudget.Droid
{
    [Activity(Label = "EasyBudget.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        DIContainer container = new DIContainer();

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            container.Register<IDataServiceHelper, DataServiceHelper>();
            container.Create<EasyBudgetDataService>();

            MobileAds.Initialize(this, "ca-app-pub-6358883513529026~8829832354");

            LoadApplication(new App());
        }
    }
}
