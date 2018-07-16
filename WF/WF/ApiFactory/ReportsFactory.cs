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
using Xamarin.Forms;

namespace WF.ApiFactory
{
    public class ReportsFactory : BaseFactory
    {
        public async Task<OperationResult<RequestType[]>> GetExcuseTypes(CancellationToken cancellation)
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
                    Analytics.TrackEvent("GetExcuseTypes", new Dictionary<string, string> {
                         { "token", "token" }});
                    return await Request<RequestType[]>("reports/ExcuseTypes", HttpMethod.Get, cancellation);
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetExcuseTypes");
                return null;
            }

        }

        public async Task<OperationResult<RequestType[]>> GetVacationTypes(CancellationToken cancellation)
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
                    Analytics.TrackEvent("GetVacationTypes", new Dictionary<string, string> {
                         { "token", "token" }});
                    return await Request<RequestType[]>("reports/VacationTypes", HttpMethod.Get, cancellation);
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetVacationTypes");
                return null;
            }

        }

        public async Task<OperationResult<bool>> SendExcuse(string token, int excuseId, long dateTicks, long startTimeTicks, long endTimeTicks, string reason, CancellationToken cancellation)
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

                    Analytics.TrackEvent("SendExcuse", new Dictionary<string, string> {
                         { "token", token},{ "excuseId",excuseId.ToString() } });

                    return await Request<bool>("reports/SendExcuse", HttpMethod.Post, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"excuseId", excuseId.ToString()},
                {"dateTicks", dateTicks.ToString()},
                {"startTimeTicks", startTimeTicks.ToString()},
                {"endTimeTicks", endTimeTicks.ToString()},
                {"reason", reason},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SendExcuse");
                return null;
            }


        }

        public async Task<OperationResult<bool>> SendVacation(string token, int vacationId, long startDateTicks, long endDateTics, string reason, CancellationToken cancellation)
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


                    Analytics.TrackEvent("SendVacation", new Dictionary<string, string> {
                         { "token", token},{ "vacationId",vacationId.ToString() } });
                    return await Request<bool>("reports/SendVacation", HttpMethod.Post, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"vacationId", vacationId.ToString()},
                {"startDateTicks", startDateTicks.ToString()},
                {"endDateTics", endDateTics.ToString()},
                {"reason", reason},
            });

                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "SendVacation");
                return null;
            }
        }

        public async Task<OperationResult<ReportRequest[]>> GetMyRequests(string token, int status, int ret, long fromTicks, long toTicks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("GetMyRequests", new Dictionary<string, string> {
                         { "token", token},{ "status",status.ToString() } });

                    return await Request<ReportRequest[]>("reports/MyRequests", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"status", status.ToString()},
                {"ret", ret.ToString()},
                {"fromTicks", fromTicks.ToString()},
                {"toTicks", toTicks.ToString()},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetMyRequests");
                return null;
            }
        }

        public async Task<OperationResult<ReportRequest[]>> RequestsForEmp(string token, string empId, long fromTicks, long toTicks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("RequestsForEmp", new Dictionary<string, string> {
                         { "token", token},{ "empId",empId } });
                    return await Request<ReportRequest[]>("reports/RequestsForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"empId", empId},
                {"fromTicks", fromTicks.ToString()},
                {"toTicks", toTicks.ToString()}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestsForEmp");
                return null;
            }
        }

        public async Task<OperationResult<bool>> RequestDecision(string token, string isVacTypes, string erqIds, string mrgId, bool approve, CancellationToken cancellation)
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
                    Analytics.TrackEvent("RequestDecision", new Dictionary<string, string> {
                         { "token", token},{ "isVacTypes ", isVacTypes } });
                    return await Request<bool>("reports/RequestDecision", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"isVacTypes", isVacTypes},
                {"erqIds", erqIds},
                {"mrgId", mrgId},
                {"approve", approve.ToString()},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "RequestDecision");
                return null;
            }
        }

    }
}
