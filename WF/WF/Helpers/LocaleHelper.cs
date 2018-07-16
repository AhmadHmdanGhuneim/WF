using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WF.DependencyServices;
using WF.Functions;
using WF.Resources;
using Xamarin.Forms;

namespace WF.Helpers
{
   public static class LocaleHelper
    {

        public static void Init()
        {
            //DependencyService.Get<ILocalize>().SetLocale(new CultureInfo(Settings.Locale));
            Settings.Culture = new CultureInfo(Settings.Locale);
            Resource.Culture = Settings.Culture;
        }



        public static async void ChangeAppLanguageAsync()
        {
            try
            {
                string language = GeneralFunctions.GetLanguage();
                LocaleItemWithCultureCode cultureLocal = new LocaleItemWithCultureCode();
                if (!language.Contains(GeneralFunctions.Language.ar.ToString()))
                {
                    cultureLocal.Country = "US";
                    cultureLocal.CountryCode = "US";
                    cultureLocal.CultureCode = "US";
                    cultureLocal.LanguageCode = "en";
                    cultureLocal.LanguageName = "English";
                }
                else
                {
                    cultureLocal.Country = "Saudi Arabia";
                    cultureLocal.CountryCode = "SA";
                    cultureLocal.CultureCode = "SA";
                    cultureLocal.LanguageCode = "ar";
                    cultureLocal.LanguageName = "Arabic";
                }
                Xamarin.Forms.Application.Current.Properties[GeneralFunctions.AppKey.lang.ToString()] = cultureLocal.LanguageCode + "-" + cultureLocal.CultureCode;
                Settings.Locale = cultureLocal.LanguageCode + "-" + cultureLocal.CultureCode;
                Settings.Culture = new CultureInfo(Settings.Locale);
                Resource.Culture = Settings.Culture;

                await Xamarin.Forms.Application.Current.SavePropertiesAsync();
                DependencyService.Get<ILocalize>().SetLocale(cultureLocal);


            }
            catch (Exception)
            {

                throw;
            }
        }



        public static void ChangeCulture()
        {

            //string language = GeneralFunctions.GetLanguage();

           // Settings.Locale =  language;

            Settings.Locale = Settings.Locale == "en-US" ? "ar-SA" : "en-US";

            LocaleHelper.Init();

            var currentLanguage = Settings.Locale.ToString();

            if (currentLanguage == "ar-SA")
            {
                ((App)Application.Current).ReloadApp("ar-SA");
            }
            else
            {
                ((App)Application.Current).ReloadApp("en-US");
            }
        }




    }
}
