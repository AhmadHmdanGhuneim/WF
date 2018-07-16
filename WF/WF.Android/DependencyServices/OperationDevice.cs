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
using WF.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(WF.Droid.DependencyServices.OperationDevice))]

namespace WF.Droid.DependencyServices
{
    public class OperationDevice : IOperationDevice
    {
        public void CloseApp()
        {
            try
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void RestartApp()
        {
            try
            {
                var context = (Forms.Context as MainActivity);
                Intent i = context.PackageManager.GetLaunchIntentForPackage(context.PackageName);
                context.StartActivity(i);

            }
            catch 
            {

                throw;
            }

        }
    }
}