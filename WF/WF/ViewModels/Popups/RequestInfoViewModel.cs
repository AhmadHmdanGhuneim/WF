using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WF.Functions;
using WF.Helpers;
using WF.Models.Reports;
using WF.Services;
using Xamarin.Forms;

namespace WF.ViewModels.Popups
{
    public class RequestInfoViewModel : ObservableObject
    {
        public ReportRequest Request { get; }

        public bool IsVac => Request.RetId == "VAC";

        public bool IsExc => !IsVac;

        public ICommand BackCommand { get; }

        public RequestInfoViewModel(ReportRequest request)
        {
            try
            {
                Request = request;
                BackCommand = new Command(Back);
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestBackViewModel");
            }
           
        }

        private async void Back()
        {
            await NavigationService.BackPopup();
        }
    }
}
