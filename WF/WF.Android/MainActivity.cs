using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Refractored.XamForms.PullToRefresh.Droid;
using Android.Content;
using WF.Droid.Gcm.Client;

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
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            //try
            //{
            //    // Check to ensure everything's set up right
            //    GcmClient.CheckDevice(this);
            //    GcmClient.CheckManifest(this);

            //    // Register for push notifications
            //    System.Diagnostics.Debug.WriteLine("Registering...");
            //    GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            //}
            //catch (Java.Net.MalformedURLException)
            //{
            //    CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            //}
            //catch (Exception e)
            //{
            //    CreateAndShowDialog(e.Message, "Error");
            //}
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

