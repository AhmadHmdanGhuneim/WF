using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Functions;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Reports;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.ViewModels.Popups;
using WF.Views.Popups;
using Xamarin.Forms;

namespace WF.ViewModels.Results
{
    public class RequestDecisionResultViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public SelectSummary SelectSummary { get; set; } = new SelectSummary();
        private readonly User _user;
        private readonly ReportsFactory _repFactory;
        public ICommand ApproveCommand { get; }

        public ICommand RejectCommand { get; }

        public ICommand ViewInfoCommand { get; }

        public ObservableCollection<ReportRequest> Requests { get; set; } = new ObservableCollection<ReportRequest>();

        public ICommand CheckCommand { get; }

        private bool _isBlockingDisplay;

        public Color FirstColor => Color.FromRgb(254, 255, 252);

        public Color SecondColor => Color.FromRgb(219, 216, 220);



        public bool IsFooterVisible => Requests.Count > 0;

        public bool IsBlockingDisplay
        {
            get { return _isBlockingDisplay; }
            set { SetProperty(ref _isBlockingDisplay, value); }
        }


        public int UserPostion => IsLtrLang ? 1 : 2;
        public int DatePostion => IsLtrLang ? 2 : 1;
        public int InfoPosition => IsLtrLang ? 3 : 0;
        public int CheckPosition => IsLtrLang ? 0 : 3;




        public RequestDecisionResultViewModel(ObservableCollection<ReportRequest> reportRequests, SelectSummary selectSummary)
        {
            try
            {


                _user = GeneralFunctions.GetUser();
                _repFactory = new ReportsFactory();
                CheckCommand = new Command<ReportRequest>(Check);
                ApproveCommand = new Command(Approve);
                RejectCommand = new Command(Reject);
                ViewInfoCommand = new Command<ReportRequest>(ViewInfo);
                Requests = reportRequests;
                SelectSummary = selectSummary;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDecisionResultViewModel");
            }

        }





        private async void ViewInfo(ReportRequest report)
        {
            try
            {
                await NavigationService.NavigatePopup(new RequestInfoViewModel(report));
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDecisionResultViewModel viewIfo");
            }
        }




        private void Approve()
        {
            try
            {
                ApproveOrReject(true);
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDecisionResultViewModel Approve");
            }
        }

        private void Reject()
        {
            try
            {
                ApproveOrReject(false);
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDecisionResultViewModel :  Reject");
            }
        }


        private async void ApproveOrReject(bool isApprove)
        {
            try
            {


                IsBlockingDisplay = true;
                var col = Requests.Where(e => e.IsSelected).ToArray();
                if (col.Any())
                {
                    var isVacTypes = new StringBuilder();
                    var erqIds = new StringBuilder();
                    foreach (var request in col)
                    {
                        erqIds.Append(request.Id + ",");
                        isVacTypes.Append((request.RetId == "VAC") + ",");
                    }
                    var res = await _repFactory.RequestDecision(_user.Token, isVacTypes.ToString().TrimEnd(','), erqIds.ToString().TrimEnd(','), _user.Login, isApprove, _cancellationToken.Token);
                    if (res.ResultCode == ResultCode.Success)
                    {
                        if (res.Data)
                        {
                            // await MessageViewer.SuccessAsync(Resource.SuccessSentRequest);
                            var sucessPage = new SuccessPopupPage("SuccessSentRequest");
                            await Rg.Plugins.Popup.Extensions.NavigationExtension.PushPopupAsync(null, sucessPage);

                            foreach (var request in col)
                            {
                                Requests.Remove(request);
                            }
                            if (Requests.Count > 0)
                            {
                                var i = 0;
                                foreach (var request in Requests)
                                {
                                    request.BackgroundColor = i++ % 2 == 0 ? FirstColor : SecondColor;
                                }
                            }
                            else
                            {
                                OnPropertyChanged(nameof(IsFooterVisible));

                            }
                        }
                        else
                        {
                            await MessageViewer.ErrorAsync(Resource.FailureSentRequst);
                        }
                    }
                }
                else
                {
                    await MessageViewer.ErrorAsync(Resource.NotSelectedError);
                }
                IsBlockingDisplay = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Check(ReportRequest req)
        {
            req.IsSelected = !req.IsSelected;
        }

        public void CancellAll()
        {
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}
