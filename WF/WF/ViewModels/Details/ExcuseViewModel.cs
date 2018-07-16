using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Functions;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Reports;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.Views.Popups;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;


namespace WF.ViewModels.Details
{
    public class ExcuseViewModel : ObservableObject, ICancellable
    {

        public int FromTimePosition => IsLtrLang ? 0 : 2;
        public int DatePosition => IsLtrLang ? 2 : 0;

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

        public DateTime MaxDate => DateTime.Today.AddMonths(6);

        public DateTime MinTime => DateTime.Today.AddMonths(-6);

        private DateTime _selectedDate = DateTime.Today;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
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

        private string _excuseTypesPikerTitle = Resource.ExcuseTypeTitle;

        public string ExcuseTypesPikerTitle
        {
            get { return _excuseTypesPikerTitle; }
            set { SetProperty(ref _excuseTypesPikerTitle, value); }
        }

        private TimeSpan _timeFrom;

        public TimeSpan TimeFrom
        {
            get { return _timeFrom; }
            set { SetProperty(ref _timeFrom, value); }
        }

        private TimeSpan _timeTo;

        public TimeSpan TimeTo
        {
            get { return _timeTo; }
            set { SetProperty(ref _timeTo, value); }
        }

        private string _reason;

        public string Reason
        {
            get { return _reason; }
            set { SetProperty(ref _reason, value); }
        }
        public ICommand ChangeLangCommand { get; }
        public ExcuseViewModel()
        {
            _factory = new ReportsFactory();
            _user = GeneralFunctions.GetUser();

            RefreshCommand = new Command(Refresh);
            SendCommand = new Command(Send);
            ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);
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
            ExcuseTypesPikerTitle = Resource.DownloadingText;
            bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
            var res = await _factory.GetExcuseTypes(_cancellationToken.Token);
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
                    if(!string.IsNullOrEmpty(req.Name))
                    {
                        RequestTypes.Add(req);
                    }
                   
                }
            }
            ExcuseTypesPikerTitle = Resource.ExcuseTypeTitle;
        }

        private async void Send()
        {
            if (SelectedRequestType == null || string.IsNullOrWhiteSpace(Reason))
            {
               // await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                List<string> messageList = new List<string>();
                messageList.Add("NotEnouthFields");
                var errorMessage = new ErrorMessagePopup(messageList);
                await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null,errorMessage);
                return;
            }
            if (TimeFrom >= TimeTo)
            {
                List<string> messageList = new List<string>();
                messageList.Add("ExcuseValidationError");
                var errorMessage = new ErrorMessagePopup(messageList);
                await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, errorMessage);
               // await MessageViewer.ErrorAsync(Resource.ExcuseValidationError);
                return;
            }

            var page = new LoadingPopupPage();
            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);

            CancellAll();
            IsIndicatorVisible = true;
            var res = await _factory.SendExcuse(_user.Token, SelectedRequestType.Id, SelectedDate.Ticks, SelectedDate.Add(TimeFrom).Ticks, SelectedDate.Add(TimeTo).Ticks, Reason, _cancellationToken.Token);


            if (res.ResultCode == ResultCode.Success)
            {
                if (res.Data)
                {
                    CloseAllPopup();
                    //await MessageViewer.SuccessAsync(Resource.SuccessSentRequest);
                    SelectedDate = DateTime.Today;
                    TimeFrom = new TimeSpan();
                    TimeTo = new TimeSpan();
                    SelectedRequestType = null;
                    Reason = "";
                    var sucessPage = new SuccessPopupPage("SuccessSentRequest");
                    await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, sucessPage);

                }
                else
                {
                    CloseAllPopup();
                    await MessageViewer.ErrorAsync(Resource.FailureSentRequst);
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

         


            IsIndicatorVisible = false;

        }


        private async void CloseAllPopup()
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.PopAllAsync();
            //await Navigation.PopAllPopupAsync();
        }
    }
}
