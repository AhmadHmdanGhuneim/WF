using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : MasterDetailPage
    {
		public MasterPage ()
		{
            

            InitializeComponent();
            if (Device.OS == TargetPlatform.iOS)
            {
                MasterBehavior = MasterBehavior.SplitOnPortrait;
            }
            App.Master = this;
        }
	}
}