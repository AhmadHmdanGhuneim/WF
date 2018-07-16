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
    public partial class RequestDecisionResultPage : ContentPage
    {
        public RequestDecisionResultPage(object viewModel)
        {
            try
            {


                InitializeComponent();
                BindingContext = viewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}