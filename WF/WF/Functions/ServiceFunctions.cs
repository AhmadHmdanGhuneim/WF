using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System;

using System.Net.Http;

using System.Threading.Tasks;
using WF.Models.Auth;
using WF.ViewModels.Auth;
using WF.Views.Auth;
using Xamarin.Forms;

namespace WF.Functions
{
    public static class ServiceFunctions
    {
#pragma warning disable CS0618 // Type or member is obsolete
        static String HOST_URL = "http://50.62.35.11:3000/";


        public static async Task<User> CheckLogin(string prmUserName, string prmPassword)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {

                   // await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));
                   ((LoginPage)((NavigationPage)(Application.Current.MainPage)).RootPage).CloseAllPopup();
                    return null;
                }
                else
                {
                    string link = "auth/signin?login=" + prmUserName + "&password=" + prmPassword + "&isAndroid=" + Device.OS == Device.Android ? "True" : "False";
                    string responseBody = await HandelServiceAsync(link, "Reset Password");
                    User user = WF.Functions.Converter.ConvetToUser(responseBody);
                    return user;
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return null;
            }
        }


        static async Task<string> HandelServiceAsync(string prmLinlService, string prmCaller)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(HOST_URL + prmLinlService, HttpCompletionOption.ResponseContentRead);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "HandelException");
                return null;
            }
        }

        public static async void CloseAllPopup()
        {
            try
            {
                await ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).Navigation.PopAllPopupAsync();
            }
            catch (Exception exceptionError)
            {
                GeneralFunctions.HandelException(exceptionError, "CloseAppPopGeneralFunction");
            }

        }
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
