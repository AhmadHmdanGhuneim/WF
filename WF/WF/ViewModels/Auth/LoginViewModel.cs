using System.Threading;
using System.Windows.Input;
using WF.ApiFactory;
using WF.DependencyServices;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Views;
using WF.Resources;
using Plugin.Connectivity;
using Plugin.Fingerprint;
using Xamarin.Forms;
using WF.Functions;
using WF.Views;
using WF.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using static WF.Functions.GeneralFunctions;
using Microsoft.AppCenter.Crashes;
using WF.CustomeControl;

namespace WF.ViewModels.Auth
{
    public class LoginViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly AuthFactory _factory;

        private User _user;

        private IFingerprint _fingerprint = DependencyService.Get<IFingerprint>();
        public ImageAlignment ImagePosition => IsLtrLang ? ImageAlignment.Left : ImageAlignment.Right;

        private string _companyId;


        public string CompayId
        {
            get { return _companyId; }
            set { SetProperty(ref _companyId, value); }
        }


        private string _login;

        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _visibleFingerprint;

        public bool VisibleFingerprint
        {
            get { return _visibleFingerprint; }
            set { SetProperty(ref _visibleFingerprint, value); }
        }


        private bool _visibleCompany;

        public bool VisibleCompany
        {
            get { return _visibleCompany; }
            set { SetProperty(ref _visibleCompany, value); }
        }


        public ICommand AuthCommand { get; }

        public ICommand FingerprintAuthCommand { get; }

        public ICommand ChangeLangCommand { get; }

        public LoginViewModel()
        {
            try
            {


                _factory = new AuthFactory();

                AuthCommand = new Command(Auth);
                FingerprintAuthCommand = new Command(FingerprintAuth);
                ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);

                if (Device.RuntimePlatform == Device.Android && VisibleFingerprint)
                {
                    _fingerprint.StartListen(FingerSuccess);
                }
                VisibleCompany = false;
                //if (GeneralFunctions.GetCompanyId() == null)
                //{
                //    VisibleCompany = true;
                //}


            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                var page = new ErrorPopup();
                Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            }

        }

        private async void Auth()
        {
            try
            {


                if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password))
                {
                    MessageViewer.Error(Resource.NotEnouthFields);
                    return;
                }






                await MessageViewer.Waiting();



                //var comany = await _factory.CheckComapny(CompayId, _cancellationToken.Token);
                //if (comany != null)
                //{
                    

                //    if (comany.Data != null)
                //    {

                        
                        var user = await _factory.SignIn(Login, Password, Device.RuntimePlatform == Device.Android, _cancellationToken.Token);

                        if (user != null)
                        {
                            if (user.ResultCode == ResultCode.Success)
                            {
                                /* Set The Main Page is MasterDetailsPage */
                                //GeneralFunctions.SaveUserValues(user.Data);

                                //Application.Current.Properties.Remove(AppKey.User.ToString());
                                //Application.Current.Properties.Add(AppKey.User.ToString(), user.Data);

                                GeneralFunctions.SaveUserValues(user.Data, Password);

                                await Application.Current.SavePropertiesAsync();
                                Application.Current.MainPage = new MasterPage();


                                //App.Realm.Write(() =>
                                //{
                                //    App.Realm.RemoveAll<User>();
                                //    App.Realm.Add(user.Data);
                                //});
                                //_user = App.Realm.All<User>().FirstOrDefault();

                                //FingerSuccess();


                            }
                            else
                            {
                                 MessageViewer.Message("", Resource.AuthFailMsg, Resource.OkText);
                            }

                        }
                //    }
                //}
                 CloseAllPopup();

            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                var page = new ErrorPopup();
                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            }
            //VisibleIndicator = false;
        }

        public void FingerSuccess()
        {
            //Device.BeginInvokeOnMainThread(delegate
            //{
            //    DependencyService.Get<IPushRegister>().Register(_user.Token);
            //});
            // NavigationService.InitMsPage();
            //NavigationService.SetDetailPage(new DashboardViewModel(), SelectedMenuOptions.Dashboard);
            try
            {
                Application.Current.MainPage = new MasterPage();
                MessagingCenter.Send(this, "Authed");
                _fingerprint.StopListen();
            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                var page = new ErrorPopup();
                Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            }

        }

        private async void FingerprintAuth()
        {
            try
            {


                if (Device.RuntimePlatform == Device.Android)
                    return;

                if (!CrossConnectivity.Current.IsConnected)
                {
                    await MessageViewer.ErrorAsync(Resource.CheckInternetMsg);
                    return;
                }

                var result = await CrossFingerprint.Current.AuthenticateAsync(Resource.FingerprintComponentMsg);
                if (result.Authenticated)
                {
                    FingerSuccess();
                }
            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                var page = new ErrorPopup();
                Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            }
        }




        public void CancellAll()
        {
            try
            {


                _cancellationToken?.Cancel();
                _cancellationToken = new CancellationTokenSource();

            }
            catch (System.Exception exception)
            {
                Crashes.TrackError(exception);
                var page = new ErrorPopup();
                Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            }
        }


        private async void CloseAllPopup()
        {
           await Rg.Plugins.Popup.Services.PopupNavigation.PopAllAsync();
          //  await Navigation.PopAllPopupAsync();
        }
    }
}
