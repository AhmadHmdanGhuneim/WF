using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WF.Helpers;

namespace WF.DependencyServices
{
   public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
#pragma warning disable CS0436 // Type conflicts with imported type
        void SetLocale(LocaleItemWithCultureCode localeItem);
    }
}
