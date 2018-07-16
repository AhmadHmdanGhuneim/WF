using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Functions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SuccessPopupPage : PopupPage
    {
		public SuccessPopupPage (string message)
		{
			InitializeComponent ();
            if(!GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString()))
            {
                SuccessMessage.HorizontalOptions = LayoutOptions.Start;
                SuccessMessage.HorizontalTextAlignment = TextAlignment.Start;
            }

            SuccessMessage.Text = GeneralFunctions.GetText(message);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            HidePopup();
        }
        private async void HidePopup()
        {
            await Task.Delay(4000);
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}