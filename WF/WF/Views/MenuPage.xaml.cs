using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Functions;
using WF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            try
            {
                InitializeComponent();
                MenuViewModel menuViewModel = new MenuViewModel();
                BindingContext = menuViewModel;
                Title = "Menu";
                Icon = Device.OS == TargetPlatform.iOS ? "HamburgerMenu.png" : null;
                ObservableCollection<MenuViewModel> observableCollection = menuViewModel.FillMenu();
                MenuListView.ItemsSource = observableCollection;
                //HeaderGrid.ItemsSource = menuViewModel.FillHeader();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MenuPage");
            }
         
        }
    }
}
