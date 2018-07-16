using WF.Models.BaseResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WF.Resources;
using WF.Views.Auth;
using WF.Services;
using WF.ViewModels.Auth;
using WF.Models;
using Xamarin.Forms;

namespace WF.Helpers
{
   public static class ApiResultExecutor
    {
        public static async Task Execute<T>(OperationResult<T> result)
        {
            switch (result.ResultCode)
            {
                case ResultCode.UnknownError:
                    await MessageViewer.ErrorAsync(Resource.CheckInternetMsg);
                    break;

                case ResultCode.AuthError:
                    await MessageViewer.ErrorAsync(Resource.AuthFailMsg);
                    break;

                case ResultCode.NoAccess:
                    await MessageViewer.ErrorAsync(Resource.NoAccessMsg);
                    break;

                case ResultCode.AuthTokenError:
                    if (!(NavigationService.CurrentPage is LoginPage))
                    {
                        //App.Realm.Write(() =>
                        //{
                        //    App.Realm.RemoveAll<User>();
                        //});

                        /*Remove all User*/
                        // NavigationService.SetDetailPage(new LoginViewModel(), SelectedMenuOptions.Login,"Login");

                        NavigationService.InitPage(new LoginViewModel());
                    }
                    break;

                default:
                    return;
            }
        }
    }
}
