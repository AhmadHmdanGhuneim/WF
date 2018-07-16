using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Helpers
{
  public class LocaleItem
    {
        public string LanguageCode { get; set; }
        public string CountryCode { get; set; }
        public string LanguageName { get; set; }
        public string Country { get; set; }

        public bool IsRestart { get; set; }

        protected LocaleItem() { }

    }
}
