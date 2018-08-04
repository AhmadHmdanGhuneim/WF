using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WF.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage(object viewModel)
        {
            try
            {

           
            InitializeComponent();
            BindingContext = viewModel;
            }
            catch (System.Exception excep)
            {

                throw;
            }

            //if (Device.RuntimePlatform == Device.Android)
            //    NavigationPage.SetHasNavigationBar(this, false);
        }


        public void CloseAllPopup()
        {
            try
            {
                
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
