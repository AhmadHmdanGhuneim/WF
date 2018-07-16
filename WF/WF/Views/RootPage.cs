using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Models;
using WF.Services;
using WF.ViewModels;
using WF.ViewModels.Auth;
using WF.Views.Auth;
using Xamarin.Forms;

namespace WF.Views
{
    public class RootPage : MasterDetailPage
    {
        private readonly MenuViewModel _masterContext;

        public RootPage()
        {
            _masterContext = new MenuViewModel();
            Master = new MenuPage()
            {
                BindingContext = _masterContext
            };
            MasterBehavior = MasterBehavior.Popover;
        }

        public void ConfirmMenuOptions(SelectedMenuOptions options)
        {
            _masterContext.ConfirmMenuOptions(options);
        }
    }
}
