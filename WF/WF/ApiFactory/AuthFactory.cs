using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;

using System.Net.Http;

using System.Threading;
using System.Threading.Tasks;
using WF.Functions;
using WF.Models.Auth;
using WF.Models.BaseResult;
using Xamarin.Forms;

namespace WF.ApiFactory
{
    public class AuthFactory : BaseFactory
    {

        public async Task<OperationResult<User>> SignIn(string login, string password, bool isAndroid, CancellationToken token)
        {
            try
            {


                if (!CrossConnectivity.Current.IsConnected)
                {

                    await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));

                    return null;
                }
                else
                {
                    Analytics.TrackEvent("SignIn", new Dictionary<string, string> {
                         { "login", login },{"password", password } });

                    return await Request<User>("auth/signin", HttpMethod.Get, token, new Dictionary<string, string>
            {
                {"login", login},
                {"password", password},
                {"isAndroid", isAndroid.ToString() }
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SignIn");
                return null;
            }

        }
    }
}
