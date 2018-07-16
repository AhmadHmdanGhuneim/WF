using Java.Util;
using System;
using System.Globalization;
using System.Threading;
using WF.DependencyServices;
using WF.Droid.DependencyServices;
using WF.Helpers;
using WF.Models.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(Localize))]
namespace WF.Droid.DependencyServices
{
    public class Localize : FormsAppCompatActivity, ILocalize
    {
        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));
            // this gets called a lot - try/catch can be expensive so consider caching or something
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e1)
            {
                // iOS locale not valid .NET culture (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new System.Globalization.CultureInfo(fallback);
                }
                catch (CultureNotFoundException e2)
                {
                    // iOS language not valid .NET culture, falling back to English
                    ci = new System.Globalization.CultureInfo("en");
                }
            }
            return ci;
        }
        string AndroidToDotnetLanguage(string androidLanguage)
        {
            var netLanguage = androidLanguage;
            //certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "ms-BN":   // "Malaysian (Brunei)" not supported .NET culture
                case "ms-MY":   // "Malaysian (Malaysia)" not supported .NET culture
                case "ms-SG":   // "Malaysian (Singapore)" not supported .NET culture
                    netLanguage = "ms"; // closest supported
                    break;
                case "in-ID":  // "Indonesian (Indonesia)" has different code in  .NET
                    netLanguage = "id-ID"; // correct code for .NET
                    break;
                case "gsw-CH":  // "Schwiizertüütsch (Swiss German)" not supported .NET culture
                    netLanguage = "de-CH"; // closest supported
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }
            return netLanguage;
        }
        string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);
            switch (platCulture.LanguageCode)
            {
                case "gsw":
                    netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }
            return netLanguage;
        }

        public void SetLocale(LocaleItemWithCultureCode localeItem)
        {
            try
            {
                Locale locale = String.IsNullOrEmpty(localeItem.CountryCode) ? new Locale(localeItem.LanguageCode) : new Locale(localeItem.LanguageCode, localeItem.CountryCode);
                Locale.Default = locale;
                var config = new global::Android.Content.Res.Configuration();
                config.Locale = locale;
                var context = global::Android.App.Application.Context;
#pragma warning disable CS0618 // Type or member is obsolete
                context.Resources.UpdateConfiguration(config, context.Resources.DisplayMetrics);
                //var ci = new System.Globalization.CultureInfo(cultureCode.LanguageCode + "-" + cultureCode.CultureCode);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr1)
                {
                    // Change locale settings in the app.
                    var dm = Forms.Context.Resources.DisplayMetrics;
                    var conf = Forms.Context.Resources.Configuration;
                    conf.Locale = new Java.Util.Locale(locale.Language);
                    Forms.Context.Resources.UpdateConfiguration(conf, dm);
#pragma warning restore CS0618 // Type or member is obsolete
                }
                if (locale.Language.Contains("ar"))
#pragma warning disable CS0618 // Type or member is obsolete
                    (Forms.Context as MainActivity).Window.DecorView.LayoutDirection = Android.Views.View.LayoutDirectionRtl;
                else
                    (Forms.Context as MainActivity).Window.DecorView.LayoutDirection = Android.Views.View.LayoutDirectionLtr;
#pragma warning restore CS0618 // Type or member is obsolete
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}