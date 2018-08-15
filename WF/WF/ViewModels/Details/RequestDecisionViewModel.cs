using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
using WF.Models.Employee;
using WF.Models.Local;
using WF.Models.Reports;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.ViewModels.Results;
using Xamarin.Forms;

namespace WF.ViewModels.Details
{
    public class RequestDecisionViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly ReportsFactory _repFactory;

        private readonly DashboardFactory _boardFactory;

        private static DateTime _now = DateTime.Now;

        private static readonly HijriDate _nowHijri = DateLocaleConvert.ConvertGregorianToHijri(DateTime.Now);

        public ObservableCollection<ReportRequest> Requests { get; set; } = new ObservableCollection<ReportRequest>();


        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }


        public Color FirstColor => Color.FromRgb(254, 255, 252);

        public Color SecondColor => Color.FromRgb(219, 216, 220);

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();



        public ObservableCollection<KeyValuePair<int, string>> Months { get; set; } = new ObservableCollection<KeyValuePair<int, string>>();

        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                SetProperty(ref _selectedYear, value);
            }
        }

        private KeyValuePair<int, string> _selectedMonth;

        public KeyValuePair<int, string> SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                SetProperty(ref _selectedMonth, value);
            }
        }



        public bool ShowAllEmployee = true;


        private bool _isAllEmps;

        public bool IsAllEmps
        {
            get { return _isAllEmps; }
            set
            {
                SetProperty(ref _isAllEmps, value);
                OnPropertyChanged(nameof(IsDepAndEmpVisible));
                if (!value && Departments.Count == 0)
                {
                    FillDepartments();
                }
            }
        }

        public bool IsDepAndEmpVisible => !IsAllEmps;



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
        public RequestDecisionViewModel()
        {
            try
            {
                _user = GeneralFunctions.GetUser();
                _repFactory = new ReportsFactory();
                _boardFactory = new DashboardFactory();

                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);

                IsAllEmps = true;
                if(!_user.IsManager)
                {
                    IsAllEmps = false;
                    ShowAllEmployee = false;
                }
                ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);
                FillCalendar();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel");
            }
        }

        private void FillCalendar()
        {
            try
            {
                string language = GeneralFunctions.GetLanguage();
                var year = _user.IsGregorianLocale ? _now.Year : _nowHijri.Year;
                var end = year - 10;
                for (var i = year; i >= end; i--)
                    Years.Add(i);

                var cul = _user.IsGregorianLocale ? new CultureInfo("en-US") : new CultureInfo("ar-SA");
                for (var i = 1; i <= 12; i++)
                {
                    string monthName = cul.DateTimeFormat.GetMonthName(i);
                    if (language.Contains(GeneralFunctions.Language.ar.ToString()))
                    {
                        cul = new CultureInfo("ar-AE");
                        monthName = cul.DateTimeFormat.GetMonthName(i);
                    }
                    Months.Add(new KeyValuePair<int, string>(i, monthName));
                }
                SelectedYear = year;
                SelectedMonth = Months.FirstOrDefault(m => m.Key == _now.Month);
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : FillCalender");
            }
        }





        private async void Refresh()
        {
            try
            {
                CancellAll();
                IsRefreshBusy = true;
                if (IsAllEmps)
                {
                    await ShowGrid();
                }
                else if (Departments.Count == 0)
                {
                    await FillDepartments();
                }
                else if (SelectedDepartment != null && Employees.Count == 0)
                {
                    await FillEmployees();
                }
                else if (SelectedEmployee != null)
                {
                    await ShowGrid();
                }
                StopRefresh();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : Refresh");
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
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : StopRefresh");
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
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : Show");
            }
        }

        private async Task ShowGrid()
        {
            try
            {


                if (!IsAllEmps && SelectedEmployee == null)
                {
                    await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                    return;
                }

                CancellAll();

                IsNoDataMsgVisible = false;
                IsIndicatorVisible = true;
                IsGridTitleVisible = false;
                await MessageViewer.Waiting();
                DateTime from;
                DateTime to;
                if (_user.IsGregorianLocale)
                {
                    from = new DateTime(SelectedYear, SelectedMonth.Key, 1);
                    to = from.AddMonths(1).AddSeconds(-1);
                }
                else
                {
                    from = DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                    {
                        Day = 1,
                        Month = SelectedMonth.Key,
                        Year = SelectedYear
                    });
                    to = from.AddDays(DateLocaleConvert.HijriDaysInMonth(SelectedYear, SelectedMonth.Key)).AddSeconds(-1);
                }

                var res = await _repFactory.RequestsForEmp(_user.Token, IsAllEmps ? string.Empty : SelectedEmployee.Id, from.Ticks, to.Ticks, _cancellationToken.Token);

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
                        selectSummary.Month = SelectedMonth.Value;
                        selectSummary.Year = SelectedYear.ToString();


                        if (IsDepAndEmpVisible)
                        {
                            selectSummary.Department = SelectedDepartment.Name;
                            selectSummary.Employee = SelectedEmployee.Name;
                        }
                        else
                        {
                            selectSummary.Department = GeneralFunctions.GetText("AllDep");
                            selectSummary.Employee = GeneralFunctions.GetText("AllEmpVar");
                        }
                        Requests = new ObservableCollection<ReportRequest>();
                        var i = 0;
                        foreach (var q in res.Data)
                        {
                            q.Calculate(_user.IsGregorianLocale);
                            q.BackgroundColor = i++ % 2 == 0 ? FirstColor : SecondColor;
                            q.EmpLoginAr = q.EmpLoginAr.Trim();
                            q.EmpLoginEn = q.EmpLoginEn.Trim();
                            Requests.Add(q);
                        }

                        NavigationService.SetDetailPage(new RequestDecisionResultViewModel(Requests, selectSummary), SelectedMenuOptions.None, "");
                    }
                }



                await MessageViewer.CloseAllPopup();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : ShowGrid");
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
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : FillDepartmet");
            }
        }

        private async Task FillEmployees()
        {
            try
            {
                CancellAll();
                EmployeePikerTitle = Resource.DownloadingText;
                bool isArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
                var res = await _boardFactory.GetEmployeesForDepartment(_user.Token, SelectedDepartment.Id, _cancellationToken.Token);
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
                        if (!string.IsNullOrEmpty(emp.Name))
                        {
                            Employees.Add(emp);
                        }

                    }
                }
                EmployeePikerTitle = Resource.EmployeeTitle;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDescitionViewModel : FillEmployee");
            }
        }

        public void CancellAll()
        {
            _cancellationToken.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}
