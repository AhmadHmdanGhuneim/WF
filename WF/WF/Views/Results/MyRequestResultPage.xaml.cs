using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Results
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyRequestResultPage : ContentPage
	{
		public MyRequestResultPage (object viewModel)
		{
			InitializeComponent ();
            BindingContext = viewModel;
		}

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}