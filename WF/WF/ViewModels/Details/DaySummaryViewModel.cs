using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Department;
using WF.Models.Employee;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using Xamarin.Forms;
using WF.Functions;

namespace WF.ViewModels.Details
{
    public class DaySummaryViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly DashboardFactory _factory;

        public int FirstImagePosition => IsLtrLang ? 0 : 4;
        public int SecandImagePosition => IsLtrLang ? 2 : 2;

        public int FirstWordPosition => IsLtrLang ? 1 : 3;
        
        public int SecandWordPosition =>IsLtrLang ? 3 :1;




        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public ObservableCollection<byte> Shifts { get; set; } = new ObservableCollection<byte>
        {
            1, 2, 3
        };

        public DateTime MaxDate => DateTime.Today;

        public DateTime MinTime => DateTime.Today.AddYears(-10);

        private DateTime _date = DateTime.Today;

        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private ShiftSummary _summary;

        public ShiftSummary Summary
        {
            get { return _summary; }
            set { SetProperty(ref _summary, value); }

        }

        private bool _isTextChartsVisible;

        public bool IsTextChartsVisible
        {
            get { return _isTextChartsVisible; }
            set { SetProperty(ref _isTextChartsVisible, value); }
        }

        private Department _selectedDepartment = null;

        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set
            {
                SetProperty(ref _selectedDepartment, value);
                if (value != null)
                    FillEmployees();
            }
        }

        private Employee _selectedEmployee = null;

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                SetProperty(ref _selectedEmployee, value);
            }
        }

        private string _departmentsPikerTitle = Resource.DepartmentTitle;

        public string DepartmentsPikerTitle
        {
            get { return _departmentsPikerTitle; }
            set { SetProperty(ref _departmentsPikerTitle, value); }
        }

        private string _employeePikerTitle = Resource.EmployeeTitle;

        public string EmployeePikerTitle
        {
            get { return _employeePikerTitle; }
            set { SetProperty(ref _employeePikerTitle, value); }
        }

        private bool _isRefreshBusy;

        public bool IsRefreshBusy
        {
            get { return _isRefreshBusy; }
            set { SetProperty(ref _isRefreshBusy, value); }
        }

        public bool IsManager => GeneralFunctions.GetUser().IsManager;

        private byte _selectedShift;

        public byte SelectedShift
        {
            get { return _selectedShift; }
            set
            {
                SetProperty(ref _selectedShift, value);
            }
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

        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public ICommand ChangeLangCommand { get; }
        public DaySummaryViewModel()
        {
            try
            {
                _user = GeneralFunctions.GetUser();
                _factory = new DashboardFactory();
                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);
                ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);
                SelectedShift = 1;

                if (IsManager)
                {
                    FillDepartments();
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummaryViewModel");
            }


        }

        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        private async Task FillDepartments()
        {
            try
            {


                CancellAll();
                DepartmentsPikerTitle = Resource.DownloadingText;
                bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
                var res = await _factory.GetDepartments(_user.Token, _cancellationToken.Token);
                if (res.ResultCode == ResultCode.Success)
                {
                    SelectedDepartment = null;
                    SelectedEmployee = null;
                    Departments.Clear();
                    Employees.Clear();
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
                GeneralFunctions.HandelException(exception, "DaySummary : FillDepartemtnt");
            }
        }

        private async Task FillEmployees()
        {
            try
            {

                CancellAll();
                EmployeePikerTitle = Resource.DownloadingText;
                bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
                var res = await _factory.GetEmployeesForDepartment(_user.Token, SelectedDepartment.Id, _cancellationToken.Token);
                if (res.ResultCode == ResultCode.Success)
                {
                    SelectedEmployee = null;
                    Employees.Clear();
                    foreach (var emp in res.Data)
                    {
                        emp.Name = emp.NameAr;
                        if (!isArabic)
                        {
                            emp.Name = emp.NameEn;
                        }
                        if(!string.IsNullOrEmpty(emp.Name))
                        {
                            Employees.Add(emp);
                        }
                       
                    }
                }
                EmployeePikerTitle = Resource.EmployeeTitle;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummary : FillEmployee");
            }
        }

        private async void Refresh()
        {
            try
            {


                CancellAll();
                IsRefreshBusy = true;
                if (IsManager && Departments.Count == 0)
                {
                    await FillDepartments();
                }
                else if (IsManager && SelectedDepartment != null && Employees.Count == 0)
                {
                    await FillEmployees();
                }
                else if (IsManager && SelectedEmployee != null || !IsManager)
                {
                    await ShowTextCharts();
                }

                StopRefresh();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummary : Refresh");
            }
        }

        private void Show()
        {
            try
            {
                ShowTextCharts();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummary : Show");
            }

        }

        private async Task ShowTextCharts()
        {
            try
            {

                if (SelectedEmployee == null && IsManager)
                {
                    await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                    return;
                }

                OperationResult<ShiftSummary> res;
                CancellAll();
                await MessageViewer.Waiting();
                //muath dassan
                //17-12-2017
                //i removed becuase at the screen it not show hijri calender when the IsGregorianLocale=false
                //so i fixed it to gregorian
                //var ticks = _user.IsGregorianLocale ? new DateTime(Date.Year, Date.Month, Date.Day).Ticks : DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                //{
                //    Year = Date.Year,
                //    Month = Date.Month,
                //    Day = Date.Day
                //}).Ticks;


                var ticks = new DateTime(Date.Year, Date.Month, Date.Day).Ticks;


                if (!IsManager)
                    res = await _factory.ShiftSummary(_user.Token, SelectedShift, ticks, _cancellationToken.Token);
                else
                    res = await _factory.ShiftSummaryForEmployee(_user.Token, SelectedEmployee.Id, SelectedShift, ticks,
                        _cancellationToken.Token);

                IsIndicatorVisible = false;
                if (res.ResultCode == ResultCode.Success)
                {
                    if (res.Data == null)
                    {
                        IsNoDataMsgVisible = true;
                        IsTextChartsVisible = false;
                    }
                    else
                    {
                        res.Data.Status = res.Data.Status?.Trim();
                        Summary = res.Data;
                        Summary.Calculate();
                        switch (Summary.Status)
                        {
                            case "P":
                                Status = Resource.StatusP;
                                break;

                            case "A":
                                Status = Resource.StatusA;
                                break;

                            case "IN":
                                Status = Resource.StatusIN;
                                break;

                            case "OU":
                                Status = Resource.StatusOU;
                                break;

                            case "OI":
                                Status = Resource.StatusOI;
                                break;

                            default:
                                Status = Resource.StatusUnknown;
                                break;
                        }
                        OnPropertyChanged(nameof(Status));
                        IsTextChartsVisible = true;
                        CancellAll();
                    }
                }
                await MessageViewer.CloseAllPopup();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummary : ShowTextCharts");
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
                GeneralFunctions.HandelException(exception, "DaySummary : FillDepartemtnt");
            }
        }
    }
}
