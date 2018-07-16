﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Details
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExcusePage : ContentPage
    {
        public ExcusePage(object viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
