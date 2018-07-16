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
	public partial class ErrorMessagePopup : PopupPage
    {
		public ErrorMessagePopup (List<string> prmErrorMessageList)
		{
			InitializeComponent ();
            if (!GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString()))
            {
                MainStackLayout.HeightRequest = 50;
                ErrorMessageLabel.HorizontalOptions = LayoutOptions.Start;
                ErrorMessageLabel.HorizontalTextAlignment = TextAlignment.Start;
            }
            foreach (string message in prmErrorMessageList)
            {
                string messgeText = GeneralFunctions.GetText(message);
                if (messgeText == null)
                {
                    messgeText = message;
                }
                ErrorMessageLabel.Text += messgeText + "\n";
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            HidePopup();
        }

        private async void HidePopup()
        {
            await Task.Delay(5000);
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}