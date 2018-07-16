﻿using System;
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

        public string IsCheckedImg => IsChecked ? "Ok.png" : "Cancel.png";

        public string InLocationImg => InLocation ? "Ok.png" : "Cancel.png";

        // bindable rows 
        private Color _bgColor;

        public Color BackgroundColor
        {
            get { return _bgColor; }
            set { SetProperty(ref _bgColor, value); }
        }

    }
}
