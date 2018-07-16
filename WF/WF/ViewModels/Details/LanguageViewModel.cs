using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using WF.DependencyServices;
using WF.Functions;
using WF.Helpers;
using WF.Resources;
using WF.Views;
using Xamarin.Forms;

namespace WF.ViewModels.Details
{
    public class LanguageViewModel : ObservableObject
    {

        public string Name { get; set; }

        public string SelectedIcon { get; set; }


        public bool IsVisible { get; set; }

        public string Key { get; set; }


        public GridLength LabelColumn => IsLtrLang ? GridLength.Star : GridLength.Auto;
        public GridLength ImageColumn => IsLtrLang ? GridLength.Auto : GridLength.Star;


        public int LabelPosition => IsLtrLang ? 0 : 1;
        public int ImagePosition => IsLtrLang ? 1 : 0;

       

        public ICommand SelectdLanguageCommand { get {return new RelayCommand(SelectLanguageAsync);}  }


        private async void SelectLanguageAsync()
        {
            try
            {
                string lang = GeneralFunctions.GetLanguage();

                if (Key == GeneralFunctions.Language.ar.ToString() && lang.Contains(GeneralFunctions.Language.en.ToString()))
                {
                    /* then the user select arabic language and the current language is English*/
                 
                    LocaleItemWithCultureCode cultureappLocal = new LocaleItemWithCultureCode();
                    cultureappLocal.Country = "Saudi Arabia";
                    cultureappLocal.CountryCode = "SA";
                    cultureappLocal.CultureCode = "SA";
                    cultureappLocal.LanguageCode = "ar";
                    cultureappLocal.LanguageName = "Arabic";
                    Xamarin.Forms.Application.Current.Properties[GeneralFunctions.AppKey.lang.ToString()] = cultureappLocal.LanguageCode + "-" + cultureappLocal.CultureCode;
                    Settings.Locale = cultureappLocal.LanguageCode + "-" + cultureappLocal.CultureCode;
                    Settings.Culture = new CultureInfo(Settings.Locale);
                    Resource.Culture = Settings.Culture;
                    await Xamarin.Forms.Application.Current.SavePropertiesAsync();
                    DependencyService.Get<ILocalize>().SetLocale(cultureappLocal);

                    if (Device.OS == TargetPlatform.Android)
                    {
                        /* Make Restart */
                        DependencyService.Get<IOperationDevice>().RestartApp();
                    }
                    else
                    {
                        App.Current.MainPage = new MasterPage();
                    }



                }
                else if(Key == GeneralFunctions.Language.en.ToString() && lang.Contains(GeneralFunctions.Language.ar.ToString()))
                {
                    /* then the user select english language and the current language is Arabic */
                    LocaleItemWithCultureCode cultureappLocal = new LocaleItemWithCultureCode();
                    cultureappLocal.Country = "US";
                    cultureappLocal.CountryCode = "US";
                    cultureappLocal.CultureCode = "US";
                    cultureappLocal.LanguageCode = "en";
                    cultureappLocal.LanguageName = "English";
                    Xamarin.Forms.Application.Current.Properties[GeneralFunctions.AppKey.lang.ToString()] = cultureappLocal.LanguageCode + "-" + cultureappLocal.CultureCode;

                    Settings.Locale = cultureappLocal.LanguageCode + "-" + cultureappLocal.CultureCode;
                    Settings.Culture = new CultureInfo(Settings.Locale);
                    Resource.Culture = Settings.Culture;
                    await Xamarin.Forms.Application.Current.SavePropertiesAsync();

                    DependencyService.Get<ILocalize>().SetLocale(cultureappLocal);
                    if (Device.OS == TargetPlatform.Android)
                    {
                        /* Make Restart */
                        DependencyService.Get<IOperationDevice>().RestartApp();
                    }
                    else
                    {
                        App.Current.MainPage = new MasterPage();
                    }
                }

            }
            catch 
            {

                throw;
            }
        }


        public ObservableCollection<LanguageViewModel> LanguageList { get; set; }


       


        public ObservableCollection<LanguageViewModel> FillLanguageMenu()
        {
            try
            {
                ObservableCollection<LanguageViewModel> languageViews = new ObservableCollection<LanguageViewModel>();
                bool isVisible = false;
                string lang = GeneralFunctions.GetLanguage();
                if(lang.Contains(GeneralFunctions.Language.ar.ToString()))
                {
                    isVisible = true;
                }

                languageViews.Add(new LanguageViewModel() {
                    Name = "العربية".Trim(),
                    Key = "ar",
                    IsVisible = isVisible,
                    SelectedIcon = "Selected.png"

                });
                isVisible = !isVisible;
                languageViews.Add(new LanguageViewModel()
                {
                    Name = "English".Trim(),
                    Key = "en",
                    IsVisible = isVisible,
                    SelectedIcon = "Selected.png"

                });


                return languageViews;



            }
            catch
            {

                throw;
            }
        }



    }
}
