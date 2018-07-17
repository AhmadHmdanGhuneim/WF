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
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using WF.Services;
using WF.ViewModels.Results;
using WF.Views.Popups;
using Xamarin.Forms;

namespace WF.ViewModels.Details
{
    public class MonthSummaryViewModel : ObservableObject, ICancellable
    {
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly User _user;

        private readonly DashboardFactory _factory;

        private static DateTime _now = DateTime.Now;

        private static readonly HijriDate _nowHijri = DateLocaleConvert.ConvertGregorianToHijri(DateTime.Now);

        public ICommand RefreshCommand { get; }

        public ICommand ShowCommand { get; }

        public ObservableCollection<AttendanceList> Summaries { get; set; } = new ObservableCollection<AttendanceList>();

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        // public ObservableCollection<Department> Departments { get; set; } = new ObservableCollection<Department>();

        public ObservableCollection<Department> Departments { get; set; }


        public ObservableCollection<KeyValuePair<int, string>> Months { get; set; } = new ObservableCollection<KeyValuePair<int, string>>();

        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();

        public ObservableCollection<int> Days { get; set; } = new ObservableCollection<int>();

        public Color FirstColor => Color.FromRgb(254, 255, 252);

        public Color SecondColor => Color.FromRgb(219, 216, 220);

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

        //private bool _isIndicatorVisible;

        //public bool IsIndicatorVisible
        //{
        //    get { return _isIndicatorVisible; }
        //    set
        //    {
        //        SetProperty(ref _isIndicatorVisible, value);
        //        OnPropertyChanged(nameof(IsButtonVisible));
        //    }
        //}

        // public bool IsButtonVisible => !_isIndicatorVisible;

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
        public MonthSummaryViewModel()
        {
            try
            {
                _user = GeneralFunctions.GetUser();
                _factory = new DashboardFactory();
                RefreshCommand = new Command(Refresh);
                ShowCommand = new Command(Show);
                FillCalendar();
                ChangeLangCommand = new Command(LocaleHelper.ChangeCulture);
                if (IsManager)
                {
                    FillDepartments();
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel");
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
                    await ShowGrid();
                }

                StopRefresh();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel : Refersh");
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
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel : StopRefresh");
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
                GeneralFunctions.HandelException(exception, "ShowGrid");
            }
        }

        private async Task FillDepartments()
        {
            try
            {

                CancellAll();
                DepartmentsPikerTitle = Resource.DownloadingText;
                Departments = new ObservableCollection<Department>();
                var res = await _factory.GetDepartments(_user.Token, _cancellationToken.Token);
                if (res.ResultCode == ResultCode.Success)
                {
                    string lang = GeneralFunctions.GetLanguage();
                    SelectedDepartment = null;
                    SelectedEmployee = null;
                    Departments.Clear();
                    Employees.Clear();
                    foreach (Department department in res.Data)
                    {
                        department.Name = department.NameAr;
                        if (!lang.Contains(GeneralFunctions.Language.ar.ToString()))
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
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel : FillDepartment");
            }
        }

        private async Task FillEmployees()
        {
            try
            {
                CancellAll();
                EmployeePikerTitle = Resource.DownloadingText;
                var res = await _factory.GetEmployeesForDepartment(_user.Token, SelectedDepartment.Id, _cancellationToken.Token);
                if (res.ResultCode == ResultCode.Success)
                {
                    string lang = GeneralFunctions.GetLanguage();
                    SelectedEmployee = null;
                    Employees.Clear();
                    foreach (var emp in res.Data)
                    {
                        emp.Name = emp.NameAr;
                        if (!lang.Contains(GeneralFunctions.Language.ar.ToString()))
                        {
                            emp.Name = emp.NameEn;
                        }

                        Employees.Add(emp);
                    }
                }
                EmployeePikerTitle = Resource.EmployeeTitle;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel : FillEmployee");
            }
        }

        private void FillCalendar()
        {
            try
            {
                string language = GeneralFunctions.GetLanguage();
                var year = _user.IsGregorianLocale ? _now.Year : _nowHijri.Year;

                //var year = GeneralFunctions.GetLanguage().Contains("en")  ? _now.Year : _nowHijri.Year;

                for (var i = year; i >= year - 10; i--)
                    Years.Add(i);

                var cul = _user.IsGregorianLocale ? new CultureInfo("en-US") : new CultureInfo("ar-SA");
                //var cul = GeneralFunctions.GetLanguage().Contains("en") ? new CultureInfo("en-US") : new CultureInfo("ar-SA");




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
                GeneralFunctions.HandelException(exception, "MonthSummaryViewModel : FillCalender");
            }
        }

        private async Task ShowGrid()
        {
            try
            {
                if (SelectedEmployee == null && IsManager)
                {
                    await MessageViewer.ErrorAsync(Resource.NotEnouthFields);
                    return;
                }

                await MessageViewer.Waiting();

                OperationResult<AttendanceList[]> res;
                CancellAll();
                Summaries.Clear();
                IsNoDataMsgVisible = false;
                // IsIndicatorVisible = true;
                IsGridTitleVisible = false;

                var d = _user.IsGregorianLocale
                    ? new DateTime(SelectedYear, SelectedMonth.Key, 1)
                    : DateLocaleConvert.ConvertHijriToGregorian(new HijriDate
                    {
                        Year = SelectedYear,
                        Month = SelectedMonth.Key,
                        Day = 1
                    });
                var startTicks = d.Ticks;
                var endTicks = _user.IsGregorianLocale
                    ? d.AddMonths(1).AddSeconds(-1).Ticks
                    : d.AddDays(DateLocaleConvert.HijriDaysInMonth(SelectedYear, SelectedMonth.Key)).AddSeconds(-1).Ticks;


                /* Call the Function to Get the result  */


                //if (!IsManager)


                if (!IsManager)
                    res = await _factory.AttendanceListViews(_user.Token, startTicks, endTicks, _cancellationToken.Token);
                else
                    res = await _factory.AttendanceListViewsForEmp(_user.Token, SelectedEmployee.Id, startTicks, endTicks, _cancellationToken.Token);

                // IsIndicatorVisible = false;

                if (res.ResultCode == ResultCode.Success)
                {
                    if (res.Data == null || res.Data.Length == 0)
                    {
                        IsNoDataMsgVisible = true;
                    }
                    else
                    {

                        IsGridTitleVisible = true;

                        var i = 0;
                        foreach (var shiftSummary in res.Data)
                        {
                            shiftSummary.Status = shiftSummary.Status?.Trim();
                            shiftSummary.Status = shiftSummary.Status == "P" ? "OW" : shiftSummary.Status;
                            shiftSummary.Calculate(_user.IsGregorianLocale, true);
                            shiftSummary.BackgroundColor = i++ % 2 == 0 ? FirstColor : SecondColor;
                            Summaries.Add(shiftSummary);
                        }
                        SelectSummary selectSummary = new SelectSummary();
                        selectSummary.Department = SelectedDepartment.Name;
                        selectSummary.Employee = SelectedEmployee.Name;
                        selectSummary.Month = SelectedMonth.Value;
                        selectSummary.Year = SelectedYear.ToString();
                        NavigationService.SetDetailPage(new MonthSummaryResultViewModel(Summaries, selectSummary), SelectedMenuOptions.None, "");
                    }
                }
                CloseAllPopup();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "Show Monthly Summary");
            }
        }

        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }


        private async void CloseAllPopup()
        {
            try
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.PopAllAsync();
            }
            catch
            {

                throw;
            }

            //await Navigation.PopAllPopupAsync();
        }



    }
}
