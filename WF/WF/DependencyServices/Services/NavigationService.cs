using System;
using System.Linq;
using System.Threading.Tasks;
using WF.Models;
using WF.Models.Views;
using WF.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using WF.Functions;

namespace WF.Services
{
    public static class NavigationService
    {


        public static Page CurrentPage => Navigation?.NavigationStack?.LastOrDefault();
        public static Page CurrentModalPage => Navigation?.ModalStack?.LastOrDefault();

        private static INavigation Navigation => Application.Current.MainPage is MasterDetailPage ? (Application.Current.MainPage as MasterDetailPage)?.Detail?.Navigation : Application.Current.MainPage?.Navigation;

        public static Task Navigate(object viewModel, SelectedMenuOptions options)
        {
            if (Navigation == null)
            {
                throw new NotSupportedException("Set navigatable Detail in master detail page before calling this.");
            }

            (CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            (CurrentModalPage?.BindingContext as ICancellable)?.CancellAll();
            var page = GetPage(viewModel);
            // App.MS?.ConfirmMenuOptions(options);
            return Navigation.PushAsync(page, true);
        }

        public static Task NavigateModal(object viewModel)
        {
            if (Navigation == null)
            {
                throw new NotSupportedException("Set navigatable Detail in master detail page before calling this.");
            }

            (CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            (CurrentModalPage?.BindingContext as ICancellable)?.CancellAll();
            var page = GetPage(viewModel);
            return Navigation.PushModalAsync(new NavigationPage(page), true);
        }

        public static Task NavigatePopup(object viewModel)
        {
            if (Navigation == null)
            {
                throw new NotSupportedException("Set navigatable Detail in master detail page before calling this.");
            }

            var page = GetPage(viewModel);
            return Navigation.PushPopupAsync((PopupPage)page);
        }

        public static async void SetDetailPage(object viewModel, SelectedMenuOptions options, string prmPageName)
        {

            try
            {
                //&& Device.OS == TargetPlatform.iOS
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
                    }

                    NavigationPage navigationPage = ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail);
                    if (GetTitlePage() != prmPageName.ToLower())
                    {
                        navigationPage.Navigation.InsertPageBefore(GetPage(viewModel), navigationPage.Navigation.NavigationStack[0]);

                        await navigationPage.Navigation.PopToRootAsync();
                        //  await   navigationPage.PushAsync(prmPage);

                    }
                }
                else
                {
                    if (GetTitlePage() != prmPageName.ToLower())
                    {
                        ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
                        Page page = GetPage(viewModel);
                        await ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).PushAsync(page, false);
                    }
                    else
                    {
                        ((MasterDetailPage)Application.Current.MainPage).IsPresented = false;
                    }
                }
            }
            catch (Exception exception)
            {

                GeneralFunctions.HandelException(exception, "OpenPage : " + prmPageName);

            }


            //(CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            //(CurrentModalPage?.BindingContext as ICancellable)?.CancellAll();
            //var page = GetPage(viewModel);
            //App.MS.Detail = new NavigationPage(page);
            //App.MS.ConfirmMenuOptions(options);
            //Application.Current.MainPage = App.MS;
        }



        private static string GetTitlePage()
        {
            try
            {
                return ((NavigationPage)((MasterDetailPage)Application.Current.MainPage).Detail).CurrentPage.Title.ToLower();
            }
            catch (Exception exception)
            {

                GeneralFunctions.HandelException(exception, "GetTitlePage");
                return "";
            }
        }

        public static void InitMsPage()
        {
            //App.MS = new RootPage();
        }

        public static void InitPage(object viewModel)
        {
            var page = GetPage(viewModel);
            Application.Current.MainPage = new NavigationPage(page);
        }

        public static async Task SetPage(object viewModel, bool fromMsPage = true)
        {
            (CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            (CurrentModalPage?.BindingContext as ICancellable)?.CancellAll();

            var page = GetPage(viewModel);
            if (fromMsPage)
            {
                //if (App.MS != null)
                //{
                //    App.MS?.ConfirmMenuOptions(SelectedMenuOptions.None);
                //    App.MS.IsPresented = false;
                //}
                await Task.Delay(500);
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(page);
            });
        }

        public static Task BackPopup()
        {
            (CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            return Navigation.PopPopupAsync();
        }

        public static Task Back()
        {
            (CurrentPage?.BindingContext as ICancellable)?.CancellAll();
            return Navigation.PopAsync(true);
        }

        public static Task BackModal()
        {
            (CurrentModalPage?.BindingContext as ICancellable)?.CancellAll();
            return Navigation.PopModalAsync(true);
        }

        public static Task Back(SelectedMenuOptions options)
        {
            //App.MS.ConfirmMenuOptions(options);
            return Navigation.PopAsync(true);
        }


        public static void ClearNavigationStack()
        {
            foreach (var page in Navigation.NavigationStack.Where(p => p.GetType() != CurrentPage.GetType()).ToList())
            {
                Navigation.RemovePage(page);
            }

            foreach (var page in Navigation.ModalStack.Where(p => p.GetType() != CurrentModalPage.GetType()).ToList())
            {
                Navigation.RemovePage(page);
            }
        }

        // All pages should follow the convention of being named the same way as their respective
        // View Models, except that the ViewModel suffix is replaced by Page.
        private static Page GetPage(object viewModel)
        {
            try
            {


                var pageType = viewModel.GetType().FullName.Replace("ViewModels", "Views").Replace("ViewModel", "Page");

                Type type = Type.GetType(pageType);
                Page page = (Page)Activator.CreateInstance(type, viewModel);

                return page;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetPage");
                throw;
            }
        }
    }
}
