using Plugin.Settings;
using Plugin.Settings.Abstractions;

using System.Globalization;


namespace WF.Helpers
{
   public static class Settings
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Locale
        {
            get { return AppSettings.GetValueOrDefault("Culture", "en-US"); }
            set
            {
                AppSettings.AddOrUpdateValue("Culture", value);
            }
        }

        private static CultureInfo _culture;

        public static CultureInfo Culture
        {
            get { return _culture == null ? _culture = new CultureInfo(Locale) : _culture; }
            set { _culture = value; }
        }

      

        public static string PushTag
        {
            get => AppSettings.GetValueOrDefault("__android_pushTag","none");
            set { AppSettings.AddOrUpdateValue("__android_pushTag", value); }
        }

        public static bool IsAuthorized
        {
            get => AppSettings.GetValueOrDefault("__android_isAuthorized", false);
            set { AppSettings.AddOrUpdateValue("__android_isAuthorized", value); }
        }



        //public static User User
        //{
        //    get { return AppSettings.GetValueOrDefault(GeneralFunctions.AppKey.User.ToString(), null); }
        //    set { AppSettings.AddOrUpdateValue(GeneralFunctions.AppKey.User.ToString(), value); }
        //}


    }
}
