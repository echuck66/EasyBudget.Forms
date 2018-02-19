using EasyBudget.Forms.Pages;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace EasyBudget.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var np = new NavigationPage(new StatusPage());

            // Toolbar
            //np.On<Windows>().SetToolbarPlacement(ToolbarPlacement.Default);

            MainPage = np;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
