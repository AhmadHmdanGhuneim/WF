using WF.ViewModels.Details;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Details
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardPage : ContentPage
    {
        //object viewModel
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = new DashboardViewModel();

        }

      
    }
}
