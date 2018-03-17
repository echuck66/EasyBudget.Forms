using System;
using Google.MobileAds;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EasyBudget.Forms.Views.AdMobView), typeof(EasyBudget.iOS.Renderers.AdMobRenderer))]

namespace EasyBudget.iOS.Renderers
{
    public class AdMobRenderer : ViewRenderer
    {
        BannerView adView;
        bool viewOnScreen;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (e.OldElement == null)
            {
                adView = new BannerView(AdSizeCons.SmartBannerPortrait)
                {
                    //AdUnitID = "ca-app-pub-6358883513529026/4028398988",
                    //AdUnitID = "ca-app-pub-3940256099942544/6300978111",
                    // Test Ad Unit
                    AdUnitID = "ca-app-pub-3940256099942544/6300978111",
                    RootViewController = GetRootViewController()
                };

                adView.AdReceived += (sender, args) =>
                {
                    if (!viewOnScreen) this.AddSubview(adView);
                    viewOnScreen = true;
                };

                var request = Request.GetDefaultRequest();

                e.NewElement.HeightRequest = GetSmartBannerDpHeight();
                adView.LoadRequest(request);

                base.SetNativeControl(adView);
            }
        }

        private UIViewController GetRootViewController()
        {
            foreach (UIWindow window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController != null)
                {
                    return window.RootViewController;
                }
            }

            return null;
        }

        private int GetSmartBannerDpHeight()
        {
            var dpHeight = (double)UIScreen.MainScreen.Bounds.Height;

            if (dpHeight <= 400) return 32;
            if (dpHeight > 400 && dpHeight <= 720) return 50;
            return 90;
        }
    }

}
