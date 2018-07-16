using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ErrorPopup : PopupPage
    {
		public ErrorPopup ()
		{
            
			InitializeComponent();
            TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped += GestureRecognizer_TappedAsync;
            MainLabel.GestureRecognizers.Add(gestureRecognizer);
        }

        private void GestureRecognizer_TappedAsync(object sender, EventArgs e)
        {
            CloseAllPopupAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            CloseAllPopupAsync();
            return base.OnBackButtonPressed();
        }

        private async void CloseAllPopupAsync()
        {
            await Navigation.PopAllPopupAsync();
        }
    }
}