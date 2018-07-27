using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Functions;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Department;
using WF.Models.Employee;
using WF.Models.Views;
using WF.Resources;
using Xamarin.Forms;


namespace WF.ViewModels.Details
{
    public class RequestSurpiseViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly DashboardFactory _boardFactory;

        private readonly SurpriseFactory _surpFactory;

        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }

        public ICommand CheckCommand { get; }

        public ICommand SendCommand { get; }

        public Color FirstColor => Color.FromRgb(254, 255, 252);

        public Color SecondColor => Color.FromRgb(219, 216, 220);


        public int NumberPosition => IsLtrLang ? 2 : 0;
        public int CheckPosition => IsLtrLang ? 0 : 2;
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        private Department _selectedDepartment = null;

        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set { SetProperty(ref _selectedDepartment, value); }
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

        public bool IsGridTitleVisible => Employees.Count > 0;

        public bool IsFooterVisible => Employees.Count > 0;

        private string _departmentsPikerTitle = Resource.DepartmentTitle;

        public string DepartmentsPikerTitle
        {
            get { return _departmentsPikerTitle; }
            set { SetProperty(ref _departmentsPikerTitle, value); }
        }

        private bool _isBlockingDisplay;

        public bool IsBlockingDisplay
        {
            get { return _isBlockingDisplay; }
            set { SetProperty(ref _isBlockingDisplay, value); }
        }
        public ICommand ChangeLangCommand { get; }
        public RequestSurpiseViewModel()
        {
            try
            {
                _user = GeneralFunctions.GetUser();
                _boardFactory = new DashboardFactory();
                _surpFactory = new SurpriseFactory();

                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);
                CheckCommand = new Command<Employee>(Check);
                SendCommand = new Command(Send);
                ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);
                FillDepartments();

                Employees.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
                {
                    OnPropertyChanged(nameof(IsGridTitleVisible));
                    OnPropertyChanged(nameof(IsFooterVisible));
                };
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel");
            }
        }

        public async void Send()
        {
            try
            {
                IsBlockingDisplay = true;

                var emps = Employees.Where(e => e.IsSelected).Select(e => e.Id).ToArray();
                if (emps.Any())
                {
                    await MessageViewer.Waiting();
                    var res = await _surpFactory.SendExcuse(_user.Token, emps, _cancellationToken.Token);
                    await MessageViewer.CloseAllPopup();
                    if (res.ResultCode == ResultCode.Success)
                    {

                        await MessageViewer.SuccessAsync(Resource.SuccessSentRequest);

                        Employees.Clear();
                    }
                    else
                    {
                        await MessageViewer.ErrorAsync(Resource.FailureSentRequst);
                    }
                }
                else
                {
                    await MessageViewer.ErrorAsync(Resource.NotSelectedError);
                }
                IsBlockingDisplay = false;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel : Send");
            }
        }

        //public  void SendPushNotification()
        //{
        //    try
        //    {
        //        string applicationID = "AIz..........Fep0";
        //        string senderId = "30............8";
        //        string deviceId = "ch_G60NPga4:APA9............T_LH8up40Ghi-J";
        //        WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //        tRequest.Method = "post";
        //        tRequest.ContentType = "application/json";
        //        var data = new
        //        {
        //            to = deviceId,
        //            notification = new
        //            {
        //                body = "Osama",
        //                title = "AlBaami",
        //                sound = "Enabled"
        //            }
        //        };

        //        var serializer = new JavaScriptSerializer();
        //        var json = serializer.Serialize(data);
        //        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //        tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
        //        tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
        //        tRequest.ContentLength = byteArray.Length;

        //        using (Stream dataStream = tRequest.GetRequestStream())
        //        {
        //            dataStream.Write(byteArray, 0, byteArray.Length);
        //            using (WebResponse tResponse = tRequest.GetResponse())
        //            {
        //                using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                {
        //                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                    {
        //                        String sResponseFromServer = tReader.ReadToEnd();
        //                        string str = sResponseFromServer;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //    }
        //}


        //private async Task SendPush(string[] emps, int msrId)
        //{
        //    var tokens =
        //    (from user in Context.LoginView
        //     join usrtoken in Context.UsrTokens on user.UsrName equals usrtoken.UsrName
        //     where emps.Contains(user.EmpID)
        //     select new
        //     {
        //         usrtoken.Token,
        //         IsAndroid = usrtoken.Platform
        //     }).ToArray();

        //    var gt = tokens.Where(e => e.IsAndroid).Select(e => e.Token.ToString()).ToArray();
        //    if (gt.Length > 0)
        //        await AzureNotifications.Post(PnsType.Gcm, gt, msrId);

        //    var at = tokens.Where(e => !e.IsAndroid).Select(e => e.Token.ToString()).ToArray();
        //    if (at.Length > 0)
        //        await AzureNotifications.Post(PnsType.Apns, at, msrId);
        //}

        private async void Refresh()
        {
            try
            {
                CancellAll();
                IsRefreshBusy = true;
                if (Departments.Count == 0)
                {
                    await FillDepartments();
                }
                else if (SelectedDepartment != null)
                {
                    await ShowGrid();
                }
                StopRefresh();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel : Referash");
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
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel :  StopeRefresh");
            }
        }

        private void Check(Employee req)
        {
            try
            {
                req.IsSelected = !req.IsSelected;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel :  Check");
            }

        }

        private void Show()
        {
            try
            {
                ShowGrid();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel :  Show");
            }
        }

        private async Task ShowGrid()
        {
            try
            {
                if (SelectedDepartment == null)
                {
                    await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                    return;
                }

                CancellAll();
                Employees.Clear();
                IsNoDataMsgVisible = false;
                IsIndicatorVisible = true;
                await MessageViewer.Waiting();
                bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
                var res = await _boardFactory.GetEmployeesForDepartment(_user.Token, SelectedDepartment.Id, _cancellationToken.Token);

                IsIndicatorVisible = false;
                if (res.ResultCode == ResultCode.Success)
                {
                    if (res.Data == null || res.Data.Length == 0)
                    {
                        IsNoDataMsgVisible = true;
                    }
                    else
                    {
                        var i = 0;
                        foreach (var q in res.Data)
                        {
                            q.Id = q.Id.Trim();
                            q.BackgroundColor = i++ % 2 == 0 ? FirstColor : SecondColor;
                            q.Name = q.NameAr;

                            if (!isArabic)
                            {
                                q.Name = q.NameEn;
                            }
                            if (!string.IsNullOrEmpty(q.Name))
                            {
                                Employees.Add(q);
                            }

                        }

                    }
                }
                await MessageViewer.CloseAllPopup();
            }
            catch
            {
                throw;
            }
        }

        private async Task FillDepartments()
        {
            try
            {
                CancellAll();
                DepartmentsPikerTitle = Resource.DownloadingText;
                bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
                var res = await _boardFactory.GetDepartments(_user.Token, _cancellationToken.Token);
                if (res.ResultCode == ResultCode.Success)
                {
                    SelectedDepartment = null;
                    Departments.Clear();
                    foreach (var department in res.Data)
                    {
                        department.Name = department.NameAr;
                        if (!isArabic)
                        {
                            department.Name = department.NameEn;
                        }
                        if (!string.IsNullOrEmpty(department.Name))
                        {
                            Departments.Add(department);
                        }

                    }
                }
                DepartmentsPikerTitle = Resource.DepartmentTitle;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestSupriseViewModel:FillDeprtment");
            }
        }

        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}
