using System;

using Android.App;
using Android.Content.PM;

using Android.Views;
using WF;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;
using Android.Content;
using WF.Droid.Gcm.Client;
using Calligraphy;
using Plugin.Connectivity;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;

namespace WF.Droid
{
    [Activity(Label = "WF", Icon = "@drawable/logo", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance = null;
        public static MainActivity CurrentActivity
        {
            get
            {
                return Instance;
            }
        }
        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;
            base.Window.RequestFeature(WindowFeatures.NoTitle);
            this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            //TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);
            PullToRefreshLayoutRenderer.Init();
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder().SetDefaultFontPath("DINNextLTArabicBold.ttf").SetFontAttrId(Resource.Attribute.fontPath).Build());
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            if (CrossConnectivity.Current.IsConnected)
            {
               // AppCenter.Start("5a27bb08-fef7-4f2b-a451-065b44d3d99e", typeof(Analytics), typeof(Crashes));


                try
                {
                    // Check to ensure everything's set up right
                    GcmClient.CheckDevice(this);
                    GcmClient.CheckManifest(this);

                    // Register for push notifications
                    System.Diagnostics.Debug.WriteLine("Registering...");
                    GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
                }
                catch (Java.Net.MalformedURLException)
                {
                    CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
                }
                catch (Exception e)
                {
                    CreateAndShowDialog(e.Message, "Error");
                }
            }
        }


        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            if (intent.HasExtra("NotificationId"))
            {
                App.RequestSurpirse(intent.GetIntExtra("NotificationId", 0));
            }
        }

        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

