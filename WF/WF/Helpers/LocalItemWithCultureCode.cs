using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Helpers
{
   public sealed class LocaleItemWithCultureCode : LocaleItem
    {
        public string CultureCode { get; protected internal set; }
    }
}
