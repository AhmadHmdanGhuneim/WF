

using WF.Droid.DependencyServices;

using Android.Content;
using Android.Locations;

using Android.Provider;
using Xamarin.Forms;
using WF.DependencyServices;

[assembly: Dependency(typeof(AskCapabilites))]
namespace WF.Droid.DependencyServices
{
    public class AskCapabilites : IAskCapabilites
    {
        public bool AskGps()
        {
            var locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

            if (!locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                var gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
                Forms.Context.StartActivity(gpsSettingIntent);
                return false;
            }
            return true;
        }
    }
}