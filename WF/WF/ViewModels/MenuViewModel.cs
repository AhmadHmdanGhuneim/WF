
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WF.Functions;
using WF.Helpers;
using WF.Models;
using WF.Models.Auth;
using WF.Models.Views;
using WF.Services;
using WF.ViewModels.Auth;
using WF.ViewModels.Details;
using WF.Views;
using WF.Views.Details;
using Xamarin.Forms;




namespace WF.ViewModels
{
    public class MenuViewModel : ObservableObject
    {
        private User _user;

        public string Title { get; set; }

        public string Icon { get; set; }

        public SelectedMenuOptions SelectedMenuOptions { get; set; }

        // public ICommand SelectMenuItemCommand { get { return new System.Windows.Input.ICommand } }

        public ICommand SelectMenuItemCommand { get { return new RelayCommand(SelectMenuItem); } }

        private string _welcome;

        public string WelcomeText
        {
            get { return _welcome; }
            set { SetProperty(ref _welcome, value); }
        }

        private readonly Color _selectedColor = Color.Black;

        private readonly Color _deactiveColor = Color.FromRgba(119, 119, 119, 255);

        private Color _dashboardColor;

        public Color DashboardColor
        {
            get { return _dashboardColor; }
            set { SetProperty(ref _dashboardColor, value); }
        }

        private Color _daySummaryColor;

        public Color DaySummaryColor
        {
            get { return _daySummaryColor; }
            set { SetProperty(ref _daySummaryColor, value); }
        }

        private Color _monthSummaryColor;

        public Color MonthSummaryColor
        {
            get { return _monthSummaryColor; }
            set { SetProperty(ref _monthSummaryColor, value); }
        }

        private Color _logoutColor;

        public Color LogoutColor
        {
            get { return _logoutColor; }
            set { SetProperty(ref _logoutColor, value); }
        }

        private Color _excuseColor;

        public Color ExcuseColor
        {
            get { return _excuseColor; }
            set { SetProperty(ref _excuseColor, value); }
        }

        private Color _vacationColor;

        public Color VacationColor
        {
            get { return _vacationColor; }
            set { SetProperty(ref _vacationColor, value); }
        }

        private Color _reqColor;

        public Color RequestColor
        {
            get { return _reqColor; }
            set { SetProperty(ref _reqColor, value); }
        }

        private Color _decColor;

        public Color DecisionColor
        {
            get { return _decColor; }
            set { SetProperty(ref _decColor, value); }
        }

        private Color _reqSurColor;

        public Color ReqSurpriseColor
        {
            get { return _reqSurColor; }
            set { SetProperty(ref _reqSurColor, value); }
        }

        private Color _surpriseResColor;

        public Color SurpriseResultColor
        {
            get { return _surpriseResColor; }
            set { SetProperty(ref _surpriseResColor, value); }
        }

        private bool _isMgr;

        public bool IsManager
        {
            get { return _isMgr; }
            set
            {
                SetProperty(ref _isMgr, value);
                OnPropertyChanged(nameof(IsEmployee));
            }
        }

        public bool IsEmployee => !IsManager;

        public MenuViewModel()
        {
            IsManager = GeneralFunctions.CheckManager(); //  _user.IsManager;
           WelcomeText = GeneralFunctions.GetUserName();
        }

       
        public enum TitlePages
        {
            mainpage,
            DaySummaryTitle,
            MonthSummaryTitle,
            ExcuseTitle,
            VacationTitle,
            RequestTitle,
            DecisionTitle,
            RequestSurpriseTitle,
            SurpriseResultTitle,
            LogoutTitle
        }



        public ObservableCollection<MenuViewModel> FillHeader()
        {
            try
            {
                ObservableCollection<MenuViewModel> HeaderListList = new ObservableCollection<MenuViewModel>();
                HeaderListList.Add(new MenuViewModel()
                {
                    
                    WelcomeText = GeneralFunctions.GetUserName()
                });
                return HeaderListList;
            }
            catch
            {

                throw;
            }
        }


        public ObservableCollection<MenuViewModel> FillMenu()
        {
            try
            {
                ObservableCollection<MenuViewModel> menuViewModelsList = new ObservableCollection<MenuViewModel>();
                //الرئيسية
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.mainpage.ToString()),
                    Icon = "dashboard.png",
                    SelectedMenuOptions = SelectedMenuOptions.Dashboard

                });

                // ملخص يومي
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.DaySummaryTitle.ToString()),
                    Icon = "daysummary.png",
                    SelectedMenuOptions = SelectedMenuOptions.DaySummary

                });
                // ملخص شهري
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.MonthSummaryTitle.ToString()),
                    Icon = "monthsummary.png",
                    SelectedMenuOptions = SelectedMenuOptions.MonthSummary
                });

                // استئذان
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.ExcuseTitle.ToString()),
                    Icon = "excuserequset.png",
                    SelectedMenuOptions = SelectedMenuOptions.Excuse
                });

                // طلب اجازة
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.VacationTitle.ToString()),
                    Icon = "vacuationrequest.png",
                    SelectedMenuOptions = SelectedMenuOptions.Vacation
                });

                // طلباتي
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.RequestTitle.ToString()),
                    Icon = "myrequset.png",
                    SelectedMenuOptions = SelectedMenuOptions.MyRequest
                });

                // اعتماد طلب
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.DecisionTitle.ToString()),
                    Icon = "requsetapproval.png",
                    SelectedMenuOptions = SelectedMenuOptions.RequestDecision
                });

                // التحضير المفاجئ
                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.RequestSurpriseTitle.ToString()),
                    Icon = "requsetsuprise.png",
                    SelectedMenuOptions = SelectedMenuOptions.RequestSurprise
                });


                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.SurpriseResultTitle.ToString()),
                    Icon = "requsetsuprise.png",
                    SelectedMenuOptions = SelectedMenuOptions.SurpriseResult
                });

                menuViewModelsList.Add(new MenuViewModel()
                {
                    Title = GeneralFunctions.GetText(TitlePages.LogoutTitle.ToString()),
                    Icon = "logout.png",
                    SelectedMenuOptions = SelectedMenuOptions.Logout
                });
                return menuViewModelsList;


            }
            catch
            {
                throw;
            }

        }


        private void SelectMenuItem()
        {
            try
            {


                //int pos = Convert.ToInt32(;
                //  App.MS.IsPresented = false;
                //if (int.TryParse(parameter as string, out pos) && pos != 0)
                //{
                //  var opt = (SelectedMenuOptions)pos;
                // (NavigationService.CurrentPage as ICancellable)?.CancellAll();

                switch (SelectedMenuOptions)
                {
                    case SelectedMenuOptions.Dashboard:
                        if (!(NavigationService.CurrentPage is DashboardPage))
                            //    App.Current.MainPage = new MasterPage();
                            //if (Device.Idiom != TargetIdiom.Tablet)
                            //{
                            //    
                            //        ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
                            //  
                            //}

                            NavigationService.SetDetailPage(new DashboardViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.DaySummary:
                        // if (!(NavigationService.CurrentPage is DaySummaryPage))
                        NavigationService.SetDetailPage(new DaySummaryViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.MonthSummary:
                        if (!(NavigationService.CurrentPage is MonthSummaryPage))
                            NavigationService.SetDetailPage(new MonthSummaryViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.Excuse:
                        if (!(NavigationService.CurrentPage is ExcusePage))
                            NavigationService.SetDetailPage(new ExcuseViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.Vacation:
                        if (!(NavigationService.CurrentPage is VacationPage))
                            NavigationService.SetDetailPage(new VacationViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.MyRequest:
                        if (!(NavigationService.CurrentPage is MyRequestsPage))
                            NavigationService.SetDetailPage(new MyRequestsViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.RequestDecision:
                        if (!(NavigationService.CurrentPage is RequestDecisionPage))
                            NavigationService.SetDetailPage(new RequestDecisionViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.RequestSurprise:
                        if (!(NavigationService.CurrentPage is RequestSurpisePage))
                            NavigationService.SetDetailPage(new RequestSurpiseViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.SurpriseResult:
                        if (!(NavigationService.CurrentPage is SurpriseResultPage))
                            NavigationService.SetDetailPage(new SurpriseResultViewModel(), SelectedMenuOptions, Title);
                        break;
                    case SelectedMenuOptions.Logout:

                        /*Remove All User */
                        Application.Current.Properties.Remove(GeneralFunctions.AppKey.UserName.ToString());
                        Application.Current.Properties.Remove(GeneralFunctions.AppKey.UserId.ToString());
                        Application.Current.Properties.Remove(GeneralFunctions.AppKey.IsManager.ToString());
                        Application.Current.SavePropertiesAsync();
                        NavigationService.InitPage(new LoginViewModel());

                        //await NavigationService.SetPage(new LoginViewModel());
                        //Device.BeginInvokeOnMainThread(delegate
                        //{
                        //    DependencyService.Get<WF.DependencyServices.IPushRegister>().Unregister();
                        //});
                        break;
                        //}
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SelectMenuItem");
            }
        }

        public void ConfirmMenuOptions(SelectedMenuOptions options)
        {
            try
            {


                DashboardColor = _deactiveColor;
                LogoutColor = _deactiveColor;
                DaySummaryColor = _deactiveColor;
                MonthSummaryColor = _deactiveColor;
                ExcuseColor = _deactiveColor;
                VacationColor = _deactiveColor;
                RequestColor = _deactiveColor;
                DecisionColor = _deactiveColor;
                ReqSurpriseColor = _deactiveColor;
                SurpriseResultColor = _deactiveColor;

                switch (options)
                {
                    case SelectedMenuOptions.Dashboard:
                        DashboardColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.DaySummary:
                        DaySummaryColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.MonthSummary:
                        MonthSummaryColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.Excuse:
                        ExcuseColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.Vacation:
                        VacationColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.MyRequest:
                        RequestColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.RequestDecision:
                        DecisionColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.RequestSurprise:
                        ReqSurpriseColor = _selectedColor;
                        break;
                    case SelectedMenuOptions.SurpriseResult:
                        SurpriseResultColor = _selectedColor;
                        break;
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SelectMenuItem");
            }

        }
    }
}
