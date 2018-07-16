using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            InitializeComponent();
            MenuViewModel menuViewModel = new MenuViewModel();
            Title = "Menu";
            Icon = Device.OS == TargetPlatform.iOS ? "HamburgerMenu.png" : null;
            ObservableCollection<MenuViewModel> observableCollection = menuViewModel.FillMenu();
            MenuListView.ItemsSource = observableCollection;
            HeaderGrid.ItemsSource = menuViewModel.FillHeader();
        }
    }
}
