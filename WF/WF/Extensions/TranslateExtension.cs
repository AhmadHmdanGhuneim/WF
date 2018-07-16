using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

using WF.Helpers;
using WF.Resources;
using System.Resources;
using WF.Functions;
using System.Reflection;

namespace WF.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private CultureInfo _ci;


        const string ResourceId = "WF.Resources.Resource";

        public TranslateExtension()
        {
            _ci = Settings.Culture;
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";
            var translation = Resource.ResourceManager.GetString(Text, _ci) ?? Text;
            return translation.Trim();
        }


        public static string Localize(string key, string comment)
        {

            // var netLanguage2 = Locale();
            var netLanguage = GeneralFunctions.GetLanguage();
            // Platform-specific
            ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
          
            string result = temp.GetString(key, new CultureInfo(netLanguage));
            return result;
        }
    }
}
