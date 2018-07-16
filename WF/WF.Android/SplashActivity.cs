using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WF.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
          
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        //protected override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        //{
        //   
        //    // Create your application here
        //}

        protected override void OnResume()
        {
            base.OnResume();
            RunOnUiThread(() =>
            {
                StartActivity(typeof(MainActivity));
            });
        }
    }
}