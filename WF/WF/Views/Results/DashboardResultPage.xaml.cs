
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.ViewModels.Results;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFShapeView;

namespace WF.Views.Results
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashboardResultPage : ContentPage
	{
		public DashboardResultPage (object viewModel)
		{
			InitializeComponent ();
            BindingContext = viewModel;
            
            var model = BindingContext as DashboardResultViewModel;
            if (model != null)
                SetPlotView();
                //model.UpdateOxyplot += SetPlotView;
        }

        public void SetPlotView()
        {
            var plot = new PlotView
            {
                WidthRequest = 250,
                HeightRequest = 250
            };

            plot.SetBinding(PlotView.ModelProperty, new Binding("OxyModel"));

            PlotGrid.Children.Clear();
            PlotGrid.Children.Add(plot);
        }
    }
}