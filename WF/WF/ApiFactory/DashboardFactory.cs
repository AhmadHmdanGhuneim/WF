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
using WF.Models.Department;
using WF.Models.Employee;
using WF.Models.Summary;
using Xamarin.Forms;

namespace WF.ApiFactory
{
    public class DashboardFactory : BaseFactory
    {
        public async Task<OperationResult<Department[]>> GetDepartments(string token, CancellationToken cancellation)
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

                    Analytics.TrackEvent("GetDepartments", new Dictionary<string, string> {
                         { "token", token }});

                    return await Request<Department[]>("Dashboard/departments", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetDepartments");
                return null;
            }

        }

        public async Task<OperationResult<Employee[]>> GetEmployeesForDepartment(string token, int depId, CancellationToken cancellation)
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
                    Analytics.TrackEvent("GetEmployeesForDepartment", new Dictionary<string, string> {
                         { "token", token },
                        { "depId", depId.ToString()} });
                    return await Request<Employee[]>("Dashboard/EmployeesForDepartment", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"depId", depId.ToString()}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "GetEmployeesForDepartment");
                return null;
            }

        }

        public async Task<OperationResult<CommonSummary>> DaySummary(string token, long ticks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("DaySummary", new Dictionary<string, string> {
                         { "token", token }});

                    return await Request<CommonSummary>("Dashboard/DaySummary", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"ticks", ticks.ToString()}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummary");
                return null;
            }


        }

        public async Task<OperationResult<CommonSummary>> DaySummaryForEmployee(string token, string empId, long ticks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("DaySummaryForEmployee", new Dictionary<string, string> {
                         { "token", token },{ "empId", empId} });

                    return await Request<CommonSummary>("Dashboard/DaySummaryForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"ticks", ticks.ToString()},
                {"empId", empId}
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DaySummaryForEmployee");
                return null;
            }

        }

        public async Task<OperationResult<CommonSummary>> MonthSummary(string token, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("MonthSummary", new Dictionary<string, string> {
                         { "token", token }});
                    return await Request<CommonSummary>("Dashboard/MonthSummary", HttpMethod.Get, cancellation, new Dictionary<string, string>
                         {
                             {"token", token},
                             {"ticksStart", ticksStart.ToString()},
                             {"ticksEnd", ticksEnd.ToString()}
                         });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MonthSummary");
                return null;
            }

        }

        public async Task<OperationResult<CommonSummary>> MonthSummaryForEmployee(string token, string empId, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("MonthSummaryForEmployee", new Dictionary<string, string> {
                         { "token", token },{ "empId", empId }});
                    return await Request<CommonSummary>("Dashboard/MonthSummaryForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
                 {
                {"token", token},
                {"ticksStart", ticksStart.ToString()},
                {"ticksEnd", ticksEnd.ToString()},
                {"empId", empId}
                });
                }

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "MonthSummaryForEmployee");
                return null;
            }



        }

        public async Task<OperationResult<ShiftSummary>> ShiftSummary(string token, byte shiftId, long ticks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("MonthSummaryForEmployee", new Dictionary<string, string> {
                         { "token", token }});

                    return await Request<ShiftSummary>("Dashboard/ShiftSummary", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"ticks", ticks.ToString()},
                {"shiftId", shiftId.ToString()}
            });
                }

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ShiftSummary");
                return null;
            }

        }

        public async Task<OperationResult<ShiftSummary>> ShiftSummaryForEmployee(string token, string empId, byte shiftId, long ticks, CancellationToken cancellation)
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
                    Analytics.TrackEvent("MonthSummaryForEmployee", new Dictionary<string, string> {
                         { "token", token },{"empId", empId } });

                    return await Request<ShiftSummary>("Dashboard/ShiftSummaryForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
                    {
                {"token", token},
                {"ticks", ticks.ToString()},
                {"empId", empId},
                {"shiftId", shiftId.ToString()}
                     });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ShiftSummaryForEmployee");
                return null;
            }

        }

        public async Task<OperationResult<ShiftSummary[]>> ShiftSummaries(string token, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("ShiftSummaries", new Dictionary<string, string> {
                         { "token", token } });

                    return await Request<ShiftSummary[]>("Dashboard/ShiftSummaries", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"ticksStart", ticksStart.ToString()},
                {"ticksEnd", ticksEnd.ToString()},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ShiftSummaries");
                return null;
            }

        }

        public async Task<OperationResult<ShiftSummary[]>> ShiftSummariesForEmployee(string token, string empId, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("ShiftSummariesForEmployee", new Dictionary<string, string> {
                         { "token", token },{"empId", empId } });

                    return await Request<ShiftSummary[]>("Dashboard/ShiftSummariesForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
                        {
                          {"token", token},
                          {"ticksStart", ticksStart.ToString()},
                          {"ticksEnd", ticksEnd.ToString()},
                          {"empId", empId},
                        });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ShiftSummariesForEmployee");
                return null;
            }

        }

        public async Task<OperationResult<AttendanceList[]>> AttendanceListViews(string token, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("AttendanceListViews", new Dictionary<string, string> {
                         { "token", token } });

                    return await Request<AttendanceList[]>("Dashboard/AttendanceListViews", HttpMethod.Get, cancellation, new Dictionary<string, string>
            {
                {"token", token},
                {"ticksStart", ticksStart.ToString()},
                {"ticksEnd", ticksEnd.ToString()},
            });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "AttendanceListViews");
                return null;
            }

        }

        public async Task<OperationResult<AttendanceList[]>> AttendanceListViewsForEmp(string token, string empId, long ticksStart, long ticksEnd, CancellationToken cancellation)
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
                    Analytics.TrackEvent("AttendanceListViewsForEmp", new Dictionary<string, string> {
                         { "token", token },{"empId", empId } });
                    return await Request<AttendanceList[]>("Dashboard/AttendanceListViewsForEmp", HttpMethod.Get, cancellation, new Dictionary<string, string>
                 {
                {"token", token},
                {"ticksStart", ticksStart.ToString()},
                {"ticksEnd", ticksEnd.ToString()},
                {"empId", empId},
                         });
                }
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "AttendanceListViewsForEmp");
                return null;
            }
        }

    }
}
