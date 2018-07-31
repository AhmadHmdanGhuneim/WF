using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Functions;
using WF.Helpers;
using WF.Models;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Reports;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.ViewModels.Results;
using Xamarin.Forms;

namespace WF.ViewModels.Details
{
    public class MyRequestsViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly ReportsFactory _factory;

        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }

        public Color FirstColor => Color.FromRgb(254, 255, 252);

        public Color SecondColor => Color.FromRgb(219, 216, 220);

        public ObservableCollection<ReportRequest> Requests { get; set; } = new ObservableCollection<ReportRequest>();

        public ObservableCollection<string> RequestTypes { get; set; } = new ObservableCollection<string>
        {
            Resource.AllDdl,
            Resource.ExcuseDdl,
            Resource.VacDdl
        };

        public ObservableCollection<string> Statuses { get; set; } = new ObservableCollection<string>
        {
            Resource.AllDdl,
            Resource.UnderProcessDdl,
            Resource.AcceptedDdl,
            Resource.RejectedDdl,
        };

        public DateTime MaxDate => DateTime.Today;

        public DateTime MinTime => DateTime.Today.AddYears(-10);

        private DateTime _dateFrom = DateTime.Today.AddDays(-7);

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetProperty(ref _dateFrom, value); }
        }

        private DateTime _dateTo = DateTime.Today;

        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetProperty(ref _dateTo, value); }
        }

        private string _selectedType = Resource.AllDdl;

        public string SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        private string _selectedStatus = Resource.UnderProcessDdl;

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { SetProperty(ref _selectedStatus, value); }
        }

        private bool _isRefreshBusy;

        public bool IsRefreshBusy
        {
            get { return _isRefreshBusy; }
            set { SetProperty(ref _isRefreshBusy, value); }
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

        private bool _isNoDataMsgVisible;

        public bool IsNoDataMsgVisible
        {
            get { return _isNoDataMsgVisible; }
            set { SetProperty(ref _isNoDataMsgVisible, value); }
        }

        private bool _isGridTitleVisible;

        public bool IsGridTitleVisible
        {
            get { return _isGridTitleVisible; }
            set { SetProperty(ref _isGridTitleVisible, value); }
        }

        public ICommand ChangeLangCommand { get; }
        public MyRequestsViewModel()
        {
            try
            {
                _user = GeneralFunctions.GetUser();
                _factory = new ReportsFactory();
                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MyRequestViewModel");
            }


        }

        private void Refresh()
        {
            try
            {
                IsRefreshBusy = true;

                // Show();

                StopRefresh();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MyRequestViewModel :  Refreash");
            }
        }

        private void StopRefresh()
        {
            try
            {
                Task.Run(async delegate
                {
                    await Task.Delay(300);
                    IsRefreshBusy = false;
                });
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MyRequestViewModel : StopRefresh");
            }

        }

        private async void Show()
        {
            try
            {
                if (DateFrom > DateTo)
                {
                    await MessageViewer.ErrorAsync(Resource.MyRequestsValidationError);
                    return;
                }

                CancellAll();
                Requests.Clear();
                IsNoDataMsgVisible = false;
                IsIndicatorVisible = true;
                IsGridTitleVisible = false;
                await MessageViewer.Waiting();
                var status = SelectedStatus == Resource.AllDdl
                    ? -1
                    : SelectedStatus == Resource.UnderProcessDdl
                        ? 0
                        : SelectedStatus == Resource.AcceptedDdl
                            ? 1
                            : 2;

                var ret = SelectedType == Resource.AllDdl
                    ? 0
                    : SelectedType == Resource.ExcuseDdl
                        ? 1
                        : 2;

                //end of day
                var to = DateTo.Add(new TimeSpan(23, 59, 59));

                var res = await _factory.GetMyRequests(_user.Token, status, ret, DateFrom.Ticks, to.Ticks, _cancellationToken.Token);

                IsIndicatorVisible = false;
                if (res.ResultCode == ResultCode.Success)
                {
                    if (res.Data == null || res.Data.Length == 0)
                    {
                        IsNoDataMsgVisible = true;
                    }
                    else
                    {
                        IsGridTitleVisible = true;
                        SelectSummary selectSummary = new SelectSummary();
                        selectSummary.ReqestType = SelectedType;
                        selectSummary.RequetStatus = SelectedStatus;
                        selectSummary.FromDate = DateFrom.Date.ToShortDateString();
                        selectSummary.ToDate = DateTo.Date.ToShortDateString();
                        var i = 0;
                        foreach (var req in res.Data)
                        {
                            req.BackgroundColor = i++ % 2 == 0 ? FirstColor : SecondColor;
                            req.Calculate();
                            req.StatusImage = req.StatusComment.ToLower() + ".png";
                            Requests.Add(req);
                        }
                        NavigationService.SetDetailPage(new MyRequestResultViewModel(Requests, selectSummary), SelectedMenuOptions.None, "");
                    }
                }
               await MessageViewer.CloseAllPopup();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MyRequestViewModel");
            }
        }

        public void CancellAll()
        {
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}
