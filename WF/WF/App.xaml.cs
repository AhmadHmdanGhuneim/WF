using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using WF.Functions;
using WF.Helpers;
using WF.Models;
using WF.Services;
using WF.ViewModels;
using WF.ViewModels.Auth;
using WF.ViewModels.Details;
using WF.ViewModels.PushActions;
using WF.Views;
using WF.Views.Auth;
using WF.Views.Details;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WF
{
    public partial class App : Application
    {
        public static MasterPage Master { get; internal set; }

        public App()
        {
            InitializeComponent();
            XamEffects.Effects.Init();
            //LocaleHelper.Init();

            LocaleHelper.ChangeAppLanguageAsync();

            if (GeneralFunctions.CheckLogin())
            {
              
                Application.Current.MainPage = new MasterPage();
            }
            else
            {
                NavigationService.InitPage(new LoginViewModel());
            }
            //MainPage = new  LoginViewModel();
            //MainPage = new MainPage();
        }


        public void ReloadApp(string language)
        {
            Settings.Locale = language;

            ////  if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            //{
            //    //LIFO is the only game in town! - so send back the last page

            //    // int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;


            //    //Page currPage = Application.Current.MainPage.Navigation.NavigationStack[index];

            //    //Page currPage = this.MainPage;




            if (NavigationService.CurrentPage is LoginPage)
            {
                NavigationService.InitPage(new LoginViewModel());
            }
            else if (NavigationService.CurrentPage is DashboardPage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new DashboardViewModel(), SelectedMenuOptions.Dashboard,GeneralFunctions.GetText(MenuViewModel.TitlePages.mainpage.ToString()));
            }
            else if (NavigationService.CurrentPage is DaySummaryPage)
            {
               // NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new DaySummaryViewModel(), SelectedMenuOptions.DaySummary, GeneralFunctions.GetText(MenuViewModel.TitlePages.DaySummaryTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is ExcusePage)
            {
               // NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new ExcuseViewModel(), SelectedMenuOptions.Excuse, GeneralFunctions.GetText(MenuViewModel.TitlePages.ExcuseTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is MonthSummaryPage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new MonthSummaryViewModel(), SelectedMenuOptions.MonthSummary, GeneralFunctions.GetText(MenuViewModel.TitlePages.MonthSummaryTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is MyRequestsPage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new MyRequestsViewModel(), SelectedMenuOptions.MyRequest, GeneralFunctions.GetText(MenuViewModel.TitlePages.RequestTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is RequestDecisionPage)
            {
               // NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new RequestDecisionViewModel(), SelectedMenuOptions.RequestDecision, GeneralFunctions.GetText(MenuViewModel.TitlePages.DecisionTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is RequestSurpisePage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new RequestSurpiseViewModel(), SelectedMenuOptions.RequestSurprise, GeneralFunctions.GetText(MenuViewModel.TitlePages.RequestSurpriseTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is SurpriseResultPage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new SurpriseResultViewModel(), SelectedMenuOptions.SurpriseResult, GeneralFunctions.GetText(MenuViewModel.TitlePages.SurpriseResultTitle.ToString()));
            }
            else if (NavigationService.CurrentPage is VacationPage)
            {
                //NavigationService.InitMsPage();
                NavigationService.SetDetailPage(new VacationViewModel(), SelectedMenuOptions.Vacation, GeneralFunctions.GetText(MenuViewModel.TitlePages.VacationTitle.ToString()));
            }
            else
            {
                NavigationService.InitPage(new LoginViewModel());
            }
        }


        public static void RequestSurpirse(int rid)
        {
            NavigationService.NavigateModal(new SurpriseAccomplishViewModel(rid));
        }
        protected override void OnStart()
        {
            // Handle when your app starts
           // AppCenter.Start("android=5a27bb08-fef7-4f2b-a451-065b44d3d99e;" +"ios=0c27caf7-db52-4b8c-bd61-ce9b41f4fea7", typeof(Analytics), typeof(Crashes));
           // Analytics.SetEnabledAsync(false);

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
