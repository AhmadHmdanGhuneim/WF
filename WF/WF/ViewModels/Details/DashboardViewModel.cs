using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.ApiFactory;
using WF.Extensions;
using WF.Helpers;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Department;
using WF.Models.Employee;
using WF.Models.Local;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Xamarin.Forms;
using WF.Functions;
using WF.ViewModels.Results;
using WF.Services;
using WF.Models;

namespace WF.ViewModels.Details
{
    public class DashboardViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public event Action UpdateOxyplot;

        private readonly User _user;

        private readonly DashboardFactory _factory;

        private static DateTime _now = DateTime.Now;

        private static readonly HijriDate _nowHijri = DateLocaleConvert.ConvertGregorianToHijri(DateTime.Now);

        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }

        public ICommand ChangeLangCommand { get; }

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public ObservableCollection<string> SummaryTypes { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<KeyValuePair<int, string>> Months { get; set; } = new ObservableCollection<KeyValuePair<int, string>>();

        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();

        public ObservableCollection<int> Days { get; set; } = new ObservableCollection<int>();

        private CommonSummary _summary;

        public CommonSummary Summary
        {
            get { return _summary; }
            set { SetProperty(ref _summary, value); }
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

        private bool _selectedDaySummaryType = false;

        public bool IsDayVisible => _selectedDaySummaryType;

        public bool IsMonthVisible => !_selectedDaySummaryType;

        public string SelectedSummaryType
        {
            get { return _selectedDaySummaryType ? Resource.DaySummaryText : Resource.MonthSummaryText; }
            set
            {
                SetProperty(ref _selectedDaySummaryType, value == Resource.DaySummaryText);
                OnPropertyChanged(nameof(IsDayVisible));
                OnPropertyChanged(nameof(IsMonthVisible));
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

        public bool IsManager => GeneralFunctions.CheckManager();
        private int _selectedDay = _now.Day;

        public int SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                SetProperty(ref _selectedDay, value);
            }
        }

        private int _selectedYear;

        public int SelectedYear
        {
            //Could not read post-migration report
            get { return _selectedYear; }
            set
            {
                SetProperty(ref _selectedYear, value);
                FillDays();
            }
        }

        private KeyValuePair<int, string> _selectedMonth;

        public KeyValuePair<int, string> SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                SetProperty(ref _selectedMonth, value);
                FillDays();
            }
        }

        private bool _isChartsVisible;

        public bool IsChartsVisible
        {
            get { return _isChartsVisible; }
            set { SetProperty(ref _isChartsVisible, value); }
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


        public int YearPosition => IsLtrLang ? 2 : 0;
        public int DayPosition => IsLtrLang ? 0 : 2;


        public string Title { get; set; }

        public DashboardViewModel()
        {
            try
            {


                _user = GeneralFunctions.GetUser();
                Title = GeneralFunctions.GetText("mainpage");
                _factory = new DashboardFactory();
                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);


                SummaryTypes = new ObservableCollection<string>
            {
                Resource.MonthSummaryText,
                Resource.DaySummaryText,
            };

                FillCalendar();

                if (IsManager)
                {
                    FillDepartments();
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardViewModel");
            }
        }

        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }

        private async Task FillDepartments()
        {
            CancellAll();
            DepartmentsPikerTitle = Resource.DownloadingText;
            bool IsArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
            var res = await _factory.GetDepartments(GeneralFunctions.GetToken(), _cancellationToken.Token);
            if (res.ResultCode == ResultCode.Success)
            {
                SelectedDepartment = null;
                SelectedEmployee = null;
                Departments.Clear();
                Employees.Clear();
                foreach (var department in res.Data)
                {
                    department.Name = department.NameAr;
                    if (!IsArabic)
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

        private async Task FillEmployees()
        {
            CancellAll();
            EmployeePikerTitle = Resource.DownloadingText;
            bool IsArabic = GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.ar.ToString());
            var res = await _factory.GetEmployeesForDepartment(GeneralFunctions.GetToken(), SelectedDepartment.Id, _cancellationToken.Token);
            if (res.ResultCode == ResultCode.Success)
            {
                SelectedEmployee = null;
                Employees.Clear();
                foreach (var emp in res.Data)
                {
                    emp.Name = emp.NameAr;
                    if (!IsArabic)
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

        private async void Refresh()
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
                await ShowCharts();
            }

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

        private void Show()
        {
            try
            {
                ShowCharts();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashBoardViewModel : Show");
            }

        }

        private async Task ShowCharts()
        {
            try
            {
                if (SelectedEmployee == null && IsManager)
                {
                    await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                    return;
                }
                await MessageViewer.Waiting();
                OperationResult<CommonSummary> res;
                CancellAll();
                IsNoDataMsgVisible = false;
                IsIndicatorVisible = true;
                if (_selectedDaySummaryType)
                {
                    var ticks = _user.IsGregorianLocale ? new DateTime(SelectedYear, SelectedMonth.Key, SelectedDay).Ticks : DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                    {
                        Year = SelectedYear,
                        Month = SelectedMonth.Key,
                        Day = SelectedDay
                    }).Ticks;

                    if (!IsManager)
                        res = await _factory.DaySummary(_user.Token, ticks, _cancellationToken.Token);
                    else
                        res = await _factory.DaySummaryForEmployee(_user.Token, SelectedEmployee.Id, ticks,
                            _cancellationToken.Token);
                }
                else
                {
                    var d = _user.IsGregorianLocale
                        ? new DateTime(SelectedYear, SelectedMonth.Key, 1)
                        : DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                        {
                            Year = SelectedYear,
                            Month = SelectedMonth.Key,
                            Day = 1
                        });
                    long startTicks = d.Ticks;
                    long endTicks = _user.IsGregorianLocale
                        ? d.AddMonths(1).Ticks
                        : DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                        {
                            Year = SelectedMonth.Key == 12 ? SelectedYear + 1 : SelectedYear,
                            Month = SelectedMonth.Key == 12 ? 1 : SelectedMonth.Key + 1,
                            Day = 1
                        }).Ticks;

                    if (!IsManager)
                        res = await _factory.MonthSummary(_user.Token, startTicks, endTicks, _cancellationToken.Token);
                    else
                        res = await _factory.MonthSummaryForEmployee(_user.Token, SelectedEmployee.Id, startTicks, endTicks, _cancellationToken.Token);
                }
                IsIndicatorVisible = false;
                if (res.ResultCode == ResultCode.Success)
                {
                    if (res.Data == null)
                    {
                        IsNoDataMsgVisible = true;
                        IsChartsVisible = false;
                    }
                    else
                    {
                        SelectSummary selectSummary = new SelectSummary();
                        selectSummary.Department = SelectedDepartment.Name;
                        selectSummary.Employee = SelectedEmployee.Name;
                        selectSummary.Month = SelectedMonth.Value;
                        selectSummary.Year = SelectedYear.ToString();

                        OperationResult<CommonSummary> operationResult = res;
                        NavigationService.SetDetailPage(new DashboardResultViewModel(selectSummary, operationResult), SelectedMenuOptions.None, "");
                    }
                }
                await MessageViewer.CloseAllPopup();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardViewModel : ShowCharts");
            }
        }





        private void FillCalendar()
        {
            try
            {
                string language = GeneralFunctions.GetLanguage();
                var year = _user.IsGregorianLocale ? _now.Year : _nowHijri.Year;
                for (var i = year; i >= year - 10; i--)
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
                GeneralFunctions.HandelException(exception, "DashboardViewModel : FillCalendar");
            }

        }

        private void FillDays()
        {
            try
            {
                if (SelectedMonth.Key == 0 || SelectedYear == 0)
                    return;

                var d = _user.IsGregorianLocale ? DateTime.DaysInMonth(SelectedYear, SelectedMonth.Key) : DateLocaleConvert.HijriDaysInMonth(SelectedYear, SelectedMonth.Key);
                if (Days.Count != d)
                {
                    Days.Clear();
                    Days.AddRange(Enumerable.Range(1, d));
                }
                SelectedDay = 1;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardViewModel : FillDays");
            }
           
        }
    }
}
