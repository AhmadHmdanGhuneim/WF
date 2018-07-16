using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WF.Functions;
using WF.Models.Auth;
using WF.Models.BaseResult;
using WF.Models.Reports;
using WF.Models.Surprise;
using Xamarin.Forms;

namespace WF.ApiFactory
{
    public class SurpriseFactory : BaseFactory
    {

        public async Task<OperationResult> SendExcuse(string token, string[] empIds, CancellationToken cancellation)
        {
            try
            {

                if (!CrossConnectivity.Current.IsConnected)
                {

                    await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));

                    return null;
                }
                else
                {
                    var emps = new StringBuilder();
                    foreach (var empId in empIds)
                    {
                        emps.Append(empId + ",");
                    }
                    Analytics.TrackEvent("AttendanceListViewsForEmp", new Dictionary<string, string> {
                         { "token", token },{"empId", empIds.Count().ToString() } });
                    return await Request<bool>("Surprise/PullSurprise", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"emps", emps.ToString().TrimEnd(',')}
            });
                }

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SendExcuse");
                return null;
            }
        }

        public async Task<OperationResult> ResendPush(string token, int msrId, CancellationToken cancellation)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {

                    await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));

                    return null;
                }
                else
                {

                    Analytics.TrackEvent("ResendPush", new Dictionary<string, string> {
                         { "token", token },{"msrId", msrId.ToString() } });
                    return await Request<bool>("Surprise/ResendPush", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"msrId", msrId.ToString()}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ResendPush");
                return null;
            }
        }

        public async Task<OperationResult> SendAttendace(string token, int msrId, double latintude, double longitude, CancellationToken cancellation)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {

                    await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));

                    return null;
                }
                else
                {
                    Analytics.TrackEvent("SendAttendace", new Dictionary<string, string> {
                         { "token", token },{"msrId", msrId.ToString() } });
                    return await Request<bool>("Surprise/PullAttendace", HttpMethod.Get, cancellation, new Dictionary<string, string>
                           {
                                 {"token", token},
                                 {"msrId", msrId.ToString()},
                                 {"latintude", latintude.ToString()},
                                 {"longitude", longitude.ToString()}
                          });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SendAttendace");
                return null;
            }
        }

        public async Task<OperationResult<MasterSurprise[]>> GetMasterSurprises(string token, CancellationToken cancellation)
        {

            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                {

                    await Application.Current.MainPage.DisplayAlert("", GeneralFunctions.GetText("connectionmessage"), GeneralFunctions.GetText("close"));

                    return null;
                }
                else
                {
                    Analytics.TrackEvent("GetMasterSurprises", new Dictionary<string, string> {
                         { "token", token }});

                    return await Request<MasterSurprise[]>("Surprise/MasterSurprises", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetMasterSurprises");
                return null;
            }



        }

        public async Task<OperationResult<DetailSurprise[]>> GetDetailSurprises(string token, int msrId, CancellationToken cancellation)
        {
            return await Request<DetailSurprise[]>("Surprise/DetailSurprises", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"msrId", msrId.ToString()},
            });
        }
    }
}
