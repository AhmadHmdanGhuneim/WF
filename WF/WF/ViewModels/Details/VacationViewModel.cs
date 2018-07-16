using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Functions;
using WF.Helpers;
using WF.Models;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Department;
using WF.Models.Reports;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.ViewModels.Auth;
using WF.Views.Popups;
using Xamarin.Forms;

namespace WF.ViewModels.Details
{
    public class VacationViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly ReportsFactory _factory;

        public ICommand RefreshCommand { get; set; }

        public ICommand SendCommand { get; set; }

        public ObservableCollection<RequestType> RequestTypes { get; set; } = new ObservableCollection<RequestType>();

        private RequestType _selectedRequestType = null;

        public RequestType SelectedRequestType
        {
            get { return _selectedRequestType; }
            set { SetProperty(ref _selectedRequestType, value); }
        }

        public DateTime MaxDate => DateTime.Today.AddYears(1);

        public DateTime MinTime => DateTime.Today.AddMonths(-6);

        private DateTime _dateFrom = DateTime.Today;

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private DateTime _dateTo = DateTime.Today.AddDays(1);

      
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private bool _isIndicatorVisible;

        public bool IsIndicatorVisible
        {
            get { return _isIndicatorVisible; }
            set
            {
                SetProperty(ref _isIndicatorVisible, value);
                OnPropertyChanged(nameof(IsButtonVisible));
            }
        }

        public bool IsButtonVisible => !_isIndicatorVisible;

        private bool _isRefreshBusy;

        public bool IsRefreshBusy
        {
            get { return _isRefreshBusy; }
            set { SetProperty(ref _isRefreshBusy, value); }
        }

        private string _vacTypesPikerTitle = Resource.VacationTypeTitle;

        public string VacationTypesPikerTitle
        {
            get { return _vacTypesPikerTitle; }
            set { SetProperty(ref _vacTypesPikerTitle, value); }
        }

        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set { SetProperty(ref _reason, value); }
        }
        public ICommand ChangeLangCommand { get; }
        public VacationViewModel()
        {
            _user = GeneralFunctions.GetUser();
            _factory = new ReportsFactory();
            RefreshCommand = new Command(Refresh);
            SendCommand = new Command(Send);
            FillReqTypes();
        }

        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        private async void Refresh()
        {
            CancellAll();
            IsRefreshBusy = true;
            if (RequestTypes.Count == 0)
                await FillReqTypes();
            StopRefresh();
        }

        private void StopRefresh()
        {
            Task.Run(async delegate
            {
                await Task.Delay(300);
                IsRefreshBusy = false;
            });
        }

        private async Task FillReqTypes()
        {
            CancellAll();
            VacationTypesPikerTitle = Resource.DownloadingText;
            bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());

            var res = await _factory.GetVacationTypes(_cancellationToken.Token);
            if (res.ResultCode == ResultCode.Success)
            {
                SelectedRequestType = null;
                RequestTypes.Clear();
                foreach (var req in res.Data)
                {
                    req.Name = req.NameAr;
                    if (!isArabic)
                    {
                        req.Name = req.NameEn;
                    }
                    if (!string.IsNullOrEmpty(req.Name))
                    {
                        RequestTypes.Add(req);
                    }

                }
            }
            VacationTypesPikerTitle = Resource.VacationTypeTitle;
        }

        private async void Send()
        {
            if (SelectedRequestType == null || string.IsNullOrWhiteSpace(Reason))
            {
                // await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                List<string> messageList = new List<string>();
                messageList.Add("NotEnouthFields");
                var errorMessage = new ErrorMessagePopup(messageList);
                await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, errorMessage);
                return;
            }
            if (DateFrom >= DateTo)
            {
                List<string> messageList = new List<string>();
                messageList.Add("VacationValidationError");
                var errorMessage = new ErrorMessagePopup(messageList);
                await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, errorMessage);
               // await MessageViewer.ErrorAsync(Resource.VacationValidationError);
                return;
            }
            var page = new LoadingPopupPage();
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);
            CancellAll();
            IsIndicatorVisible = true;
            var res = await _factory.SendVacation(_user.Token, SelectedRequestType.Id, DateFrom.Ticks, DateTo.Ticks, Reason, _cancellationToken.Token);
            if (res.ResultCode == ResultCode.Success)
            {
                if (res.Data)
                {
                    CloseAllPopup();
                    //await NavigationService.CurrentPage.DisplayAlert(Resource.SuccessText, Resource.SuccessSentRequest, Resource.OkText);
                    DateFrom = DateTime.Today;
                    DateTo = DateTime.Today.AddDays(1);
                    SelectedRequestType = null;
                    Reason = "";
                    var sucessPage = new SuccessPopupPage("SuccessSentRequest");
                    await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, sucessPage);
                }
                else
                {

                    await NavigationService.CurrentPage.DisplayAlert(Resource.ErrorText, Resource.FailureSentRequst, Resource.OkText);
                }
            }
            else if (res.ResultCode == ResultCode.Vac_DaysLimit)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_DaysLimit, Resource.OkText);
            }
            else if (res.ResultCode == ResultCode.Vac_HaveTrans)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_HaveTrans, Resource.OkText);
            }
            else if (res.ResultCode == ResultCode.Vac_MaxDays)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_MaxDays, Resource.OkText);
            }
            else if (res.ResultCode == ResultCode.Vac_Nesting)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_Nesting, Resource.OkText);
            }
            else if (res.ResultCode == ResultCode.Vac_NoEnterReset)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_NoEnterReset, Resource.OkText);
            }
            else if (res.ResultCode == ResultCode.Vac_NoWork)
            {
                CloseAllPopup();
                await NavigationService.CurrentPage.DisplayAlert(Resource.ProcessStop, Resource.Vac_NoWork, Resource.OkText);
            }

            
        }

        private async void CloseAllPopup()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAllAsync();
            //await Navigation.PopAllPopupAsync();
        }

    }
}
