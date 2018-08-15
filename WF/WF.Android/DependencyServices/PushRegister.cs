using System;
using WF.Azure;
using WF.Helpers;
using WF.DependencyServices;
using WF.Droid.DependencyServices;
using WF.Droid.Gcm.Client;
using Xamarin.Forms;
using Android.App;
using Android.Content;

[assembly: Dependency(typeof(PushRegister))]
namespace WF.Droid.DependencyServices
{
    public class PushRegister : IPushRegister
    {
        public void Register(string tag)
        {
            //Check to ensure everything's set up right
            GcmClient.CheckDevice(MainActivity.Instance);
            GcmClient.CheckManifest(MainActivity.Instance);

            // Register for push notifications
            GcmClient.Register(MainActivity.Instance, AzureConstants.AndroidSenderID);
            Settings.IsAuthorized = true;
            Settings.PushTag = tag;
          
        }

        public void Unregister()
        {
            //Settings.PushTag = string.Empty;
            //GcmClient.UnRegister(MainActivity.Instance);
            //try
            //{
            //    PushHandlerService.Hub?.Unregister();
            //}
            //catch (Exception e)
            //{
            //}
            //Settings.IsAuthorized = false;

        }


        public string GetRegistrationId()
        {
            try
            {
                var context = MainActivity.Instance;
                var registrationId = GcmClient.GetRegistrationId(context);

               
                return registrationId;
            }
            catch (Exception)
            {

                throw;
            }
        }

       
    }
}