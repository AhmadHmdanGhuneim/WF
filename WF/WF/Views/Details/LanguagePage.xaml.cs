using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.ViewModels.Details;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Details
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguagePage : ContentPage
    {
        public LanguagePage()
        {
            InitializeComponent();
            LanguageViewModel languageViewModel = new LanguageViewModel();
            ObservableCollection<LanguageViewModel> languageViewModelsList = languageViewModel.FillLanguageMenu();
            LanuguageListView.ItemsSource = languageViewModelsList;


        }
    }
}