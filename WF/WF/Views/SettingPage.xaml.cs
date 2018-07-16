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
	public partial class SettingPage : ContentPage
	{
		public SettingPage ()
		{
			InitializeComponent ();
            SettingViewModel settingViewModel = new SettingViewModel();
            ObservableCollection<SettingViewModel> settingViewModels = settingViewModel.FillSettingMenu();
            SettingListView.ItemsSource = settingViewModels;

        }
	}
}