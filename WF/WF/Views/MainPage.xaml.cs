using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WF.Views.Details;
using WF.ViewModels.Details;
using WF.Functions;
using WF.DependencyServices;

namespace WF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
    {
		public MainPage ()
		{
			InitializeComponent ();
            Title = GeneralFunctions.GetText("workforce");
            CurrentPage = Children[0];
        }



        protected override bool OnBackButtonPressed()
        {

            try
            {
                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IOperationDevice>().CloseApp();
                return base.OnBackButtonPressed();
            }
            catch (Exception exception)
            {
                
                return base.OnBackButtonPressed();
            }
        }
    }
}