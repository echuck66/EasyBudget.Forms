using EasyBudget.Forms.Pages;
using EasyBudget.Forms.Utility;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace EasyBudget.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DIContainer injectionContainer = new DIContainer();

            injectionContainer.Create<ChartUtility>();
            injectionContainer.Create<ChartColors>();

            var np = new NavigationPage(new StatusPage());

            // Toolbar
            //np.On<Windows>().SetToolbarPlacement(ToolbarPlacement.Default);
            np.BarBackgroundColor = Color.FromHex("#4db140");
            np.BarTextColor = Color.White;

            MainPage = np;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=425d2198-e1e0-4ba5-94d3-5ef8e8563568;" +
                            "ios=b338cbfd-ce78-4164-aa9b-ea14f86e9913",
                            typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
