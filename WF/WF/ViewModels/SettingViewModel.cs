
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

using System.Windows.Input;
using WF.Functions;
using WF.Helpers;
using WF.Services;
using WF.ViewModels.Auth;
using WF.ViewModels.Details;
using WF.Views.Details;
using Xamarin.Forms;

namespace WF.ViewModels
{
    public class SettingViewModel : ObservableObject
    {

      
        //public new bool IsLtrLang => Settings.Culture.Name == "en-US";
                                               /* english : Arabic   */       
        public new int StartSpan => IsLtrLang ? 1: 0;
                                       /* english : Arabic   */
        public new int EndSpan => IsLtrLang ? 0: 1;
                                            /* english : Arabic   */
        public new GridLength StartGridLength => IsLtrLang ? GridLength.Auto : GridLength.Star;

        public new GridLength EndGridLength => IsLtrLang ? GridLength.Star : GridLength.Auto;

        public string Title { get; set; }

        public string Icon { get; set; }
        public ICommand OpenPageCommand { get { return new RelayCommand(OpenPage); }  }
        private void OpenPage()
        {
            try
            {
                /* Then cLick the language button */
                if(Title == GeneralFunctions.GetText("language"))
                {
                    NavigationPage navigationPage = ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail);
                    navigationPage.PushAsync(new LanguagePage());
                    //  NavigationService.SetDetailPage(new LanguageViewModel(), Models.SelectedMenuOptions.Lanugage, GeneralFunctions.GetText("lanugage"));
                }
                else
                {
                    Application.Current.Properties.Remove(GeneralFunctions.AppKey.User.ToString());
                    Application.Current.SavePropertiesAsync();
                    NavigationService.InitPage(new LoginViewModel());
                }
                
            }
            catch 
            {

                throw;
            }
        }



        public ObservableCollection<SettingViewModel> FillSettingMenu()
        {
            try
            {


                ObservableCollection<SettingViewModel> settingViewModels = new ObservableCollection<SettingViewModel>();
                settingViewModels.Add(new SettingViewModel()
                {
                  
                    Title = GeneralFunctions.GetText("language"),
                    Icon = "Langauge.png"

                });
                settingViewModels.Add(new SettingViewModel()
                {
                    Title = GeneralFunctions.GetText("LogoutTitle"),
                    Icon = "Logout.png"

                });

                return settingViewModels;
            }
            catch 
            {

                throw;
            }
        }
        

    }
}
