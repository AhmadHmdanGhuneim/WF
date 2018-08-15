using Microsoft.AppCenter.Crashes;
using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using WF.Helpers;
using WF.Models.Auth;
using WF.Resources;
using Xamarin.Forms;
using WF.Views.Popups;

namespace WF.Functions
{
    public static class GeneralFunctions
    {
        public enum AppKey
        {
            CompanyId,
            Password,
            lang,
            UserId,
            IsManager,
            UserName,
            User,
            IsGregorianLocale,
            Token
        }

       public enum Language
        {
            ar, en
        }

        public static string GetLanguage()
        {
            try
            {


                string lang = "";
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.lang.ToString()))
                {
                    lang = Xamarin.Forms.Application.Current.Properties[AppKey.lang.ToString()].ToString();
                }
                else
                {
                    lang = "ar-SA";
                    Xamarin.Forms.Application.Current.Properties[AppKey.lang.ToString()] = lang;

                }
               
                return lang.ToString();
            }
            catch(Exception exception)
            {
                HandelException(exception, "GetLanguage");
                return "ar-SA";
            }
        }

        public static string GetText(string text)
        {
            try
            {



                string translat = Extensions.TranslateExtension.Localize(text, text);

                return translat;
            }
            catch (System.Exception exception)
            {
                HandelException(exception, "GetText");
                return text;
            }
        }

        public static async void HandelException(Exception prmException, string prmCaller)
        {
            try
            {
                MessageViewer.CloseAllPopupAsync();
                if (IsNetworkException(prmException))
                {
                    //CheckInternetMsg
                    await MessageViewer.ErrorAsync(Resource.CheckInternetMsg);

                }
                else
                {
                    Crashes.TrackError(prmException);
                    var page = new ErrorPopup();
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(page);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static string ExceptionMessage(Exception prmException)
        {
            try
            {
                string msg = "";
                if (prmException.InnerException == null)
                {
                    msg = prmException.Message;


                }
                else
                {
                    msg = prmException.InnerException.ToString();


                }
                return msg;
            }
            catch (Exception)
            {

                throw;
            }
         
        }

        public static bool IsNetworkException(Exception prmException)
        {
            try
            {
                string msg = ExceptionMessage(prmException);

                return (msg.ToLower().Contains("System.TimeoutException".ToLower())
                     || msg.ToLower().Contains("System.ServiceModel.CommunicationException".ToLower())
                     || msg.ToLower().Contains("System.ServiceModel.EndpointNotFoundException".ToLower())
                     || msg.ToLower().Contains("System.ComponentModel.AsyncCompletedEventArgs".ToLower())
                     || msg.ToLower().Contains("System.Net.Sockets.SocketException".ToLower())
                     || msg.ToLower().Contains("Network".ToLower())
                     || msg.ToLower().Contains("Socket".ToLower())
                     || msg.ToLower().Contains("System.Net.Http.HttpResponseMessage".ToLower()));


            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool CheckLogin()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey(AppKey.UserName.ToString()))
                {
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return false;
            }
        }

        public static bool CheckManager()
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.IsManager.ToString()))
                {
                    return Convert.ToBoolean(Xamarin.Forms.Application.Current.Properties[AppKey.IsManager.ToString()]);

                }
                return false;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return false;
            }
        }


        public static string GetUserName()
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.UserName.ToString()))
                {
                    return Xamarin.Forms.Application.Current.Properties[AppKey.UserName.ToString()].ToString();

                }
                return "";
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return "";
            }
        }


        public static string GetToken()
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.Token.ToString()))
                {
                    return Xamarin.Forms.Application.Current.Properties[AppKey.Token.ToString()].ToString();

                }
                return "";
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return "";
            }
        }


        public static bool GetIsGregorianLocale()
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.IsGregorianLocale.ToString()))
                {
                    return Convert.ToBoolean(Xamarin.Forms.Application.Current.Properties[AppKey.IsGregorianLocale.ToString()]);

                }
                return false;
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "CheckLogin");
                return false;
            }
        }

        public static User GetUser()
        {
            try
            {
                User user = new User();
                user.IsGregorianLocale = GetIsGregorianLocale();
                user.IsManager = CheckManager();
                user.FullName = GetUserName();
                user.Token = GetToken();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }




        public static void SaveUserValues(User prmUser,string prmPassword)
        {
            try
            {

                RemoveAllUserKey();

                Application.Current.Properties.Add(GeneralFunctions.AppKey.IsGregorianLocale.ToString(), prmUser.IsGregorianLocale);
                Application.Current.Properties.Add(GeneralFunctions.AppKey.Password.ToString(), prmPassword);
                Application.Current.Properties.Add(GeneralFunctions.AppKey.Token.ToString(), prmUser.Token);
                Application.Current.Properties.Add(GeneralFunctions.AppKey.UserId.ToString(), prmUser.EmpId);
                Application.Current.Properties.Add(GeneralFunctions.AppKey.IsManager.ToString(), prmUser.IsManager);
                Application.Current.Properties.Add(GeneralFunctions.AppKey.UserName.ToString(), prmUser.FullName);
            }
            catch (Exception exception )
            {
                HandelException(exception, "GetUser");
            }
        }


        public static string GetCompanyId()
        {
            try
            {
                if (Xamarin.Forms.Application.Current.Properties.ContainsKey(AppKey.IsGregorianLocale.ToString()))
                {
                    return Xamarin.Forms.Application.Current.Properties[AppKey.CompanyId.ToString()].ToString();
                }
                return null;
            }
            catch (Exception exception)
            {
                HandelException(exception, "GetCompanyId");
                return null;
            }
        }


        public static void RemoveAllUserKey()
        {
            try
            {
                Application.Current.Properties.Remove(AppKey.IsGregorianLocale.ToString());
                Application.Current.Properties.Remove(GeneralFunctions.AppKey.Token.ToString());
                Application.Current.Properties.Remove(GeneralFunctions.AppKey.Password.ToString());
                Application.Current.Properties.Remove(GeneralFunctions.AppKey.UserName.ToString());
                Application.Current.Properties.Remove(GeneralFunctions.AppKey.UserId.ToString());
                Application.Current.Properties.Remove(GeneralFunctions.AppKey.IsManager.ToString());

            }
            catch 
            {

                throw;
            }
        }


    }
}
