using System;
using System.Collections.Generic;
using System.Text;
using WF.Helpers;
using Xamarin.Forms;

namespace WF.Models.Surprise
{
    public class DetailSurprise : ObservableObject
    {

        public int Id { get; set; }

        public int DetailId { get; set; }

        public string EmpId { get; set; }

        public string EmpName { get; set; }

        public bool IsChecked { get; set; }

        public bool InLocation { get; set; }

        public string IsCheckedImg => IsChecked ? "ok.png" : "cancel.png";

        public string InLocationImg => InLocation ? "ok.png" : "cancel.png";

        // bindable rows 
        private Color _bgColor;

        public Color BackgroundColor
        {
            get { return _bgColor; }
            set { SetProperty(ref _bgColor, value); }
        }

        public int NumberPosition => IsLtrLang ? 0 : 3;
        public int NamePosition => IsLtrLang ? 1 : 2;

        public int CheckPosition => IsLtrLang ? 2 : 1;

        public int InPosition => IsLtrLang ? 3 : 0;



    }
}
