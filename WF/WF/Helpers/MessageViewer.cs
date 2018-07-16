
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using WF.Resources;
using WF.Services;
using WF.Views.Popups;
using Xamarin.Forms;

namespace WF.Helpers
{
    public static class MessageViewer
    {
        public static async Task MessageAsync(string title, string msg, string cancel)
        {
            await NavigationService.CurrentPage.DisplayAlert(title, msg, cancel);
        }

        public static async Task SuccessAsync(string msg)
        {
            await NavigationService.CurrentPage.DisplayAlert(Resource.SuccessText, msg, Resource.OkText);
        }

        public static async Task ErrorAsync(string msg)
        {
            await NavigationService.CurrentPage.DisplayAlert(Resource.ErrorText, msg, Resource.OkText);
        }

        public static async void CloseAllPopupAsync()
        {
            await ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).Navigation.PopAllPopupAsync();
        }


        public static void Message(string title, string msg, string cancel)
        {
            NavigationService.CurrentPage.DisplayAlert(title, msg, cancel);
        }

        public static void Success(string msg)
        {
            NavigationService.CurrentPage.DisplayAlert(Resource.SuccessText, msg, Resource.OkText);
        }

        public static void Error(string msg)
        {
            NavigationService.CurrentPage.DisplayAlert(Resource.ErrorText, msg, Resource.OkText);
        }

        public static async Task Waiting()
        {
            try
            {
                var page = new LoadingPopupPage();
                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);
            }
            catch
            {

                throw;
            }
        }


        public static async Task CloseAllPopup()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAllAsync();
        }

        public static async Task<bool> Alert(string msg)
        {
            return await NavigationService.CurrentPage.DisplayAlert(Resource.AlertActionTitle, msg, Resource.OkText, Resource.CancelToolbar);
        }
    }
}
