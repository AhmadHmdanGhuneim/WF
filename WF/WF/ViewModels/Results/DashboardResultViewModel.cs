using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using WF.Functions;
using WF.Models.BaseResult;
using WF.Models.Summary;
using WF.Models.Views;
using WF.Resources;
using Xamarin.Forms;

namespace WF.ViewModels.Results
{
    public class DashboardResultViewModel : WF.Helpers.ObservableObject, ICancellable
    {

        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public event Action UpdateOxyplot;



        public SelectSummary SelectSummary { get; set; } = new SelectSummary();

        public OperationResult<CommonSummary> ResultData { get; set; } = new OperationResult<CommonSummary>();

        public CommonSummary Summary { get; set; } = new CommonSummary();



        private string _status;

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private PlotModel _oxyModel;

        public PlotModel OxyModel
        {
            get { return _oxyModel; }
            set { SetProperty(ref _oxyModel, value); }
        }


        public DashboardResultViewModel(SelectSummary prmSelectdSummary, OperationResult<CommonSummary> prmCommonSummary)
        {
            try
            {

                SelectSummary = prmSelectdSummary;
                ResultData = prmCommonSummary;
                FillSummary();
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardResultViewModel");
            }
        }


        private void FillChartsAnimation(double totalWorkingHoursPercent, double totalAbsentDaysPercent, double totalLateHoursPercent, CancellationToken token)
        {
            try
            {

                var t = 100;

                Summary.TotalAbsentDaysPercent = 0;
                Summary.TotalLateHoursPercent = 0;
                Summary.TotalWorkingHoursPercent = 0;
                OnPropertyChanged(nameof(Summary));

                var twhp = totalWorkingHoursPercent / t;
                var tadp = totalAbsentDaysPercent / t;
                var tlhp = totalLateHoursPercent / t;

                var c = 1;
                Device.StartTimer(TimeSpan.FromMilliseconds(10), delegate
                {
                    if (token.IsCancellationRequested)
                        return false;
                    if (c == t)
                    {
                        Summary.TotalAbsentDaysPercent = totalAbsentDaysPercent;
                        Summary.TotalLateHoursPercent = totalLateHoursPercent;
                        Summary.TotalWorkingHoursPercent = totalWorkingHoursPercent;
                        OnPropertyChanged(nameof(Summary));
                        return false;
                    }

                    c++;
                    Summary.TotalAbsentDaysPercent += tadp;
                    Summary.TotalLateHoursPercent += tlhp;
                    Summary.TotalWorkingHoursPercent += twhp;
                    OnPropertyChanged(nameof(Summary));
                    return true;
                });

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardResultViewModel : FillChartsAnimation");
            }


        }
        private bool _isChartsVisible;

        public bool IsChartsVisible
        {
            get { return _isChartsVisible; }
            set { SetProperty(ref _isChartsVisible, value); }
        }


        private void FillSummary()
        {
            try
            {
                ResultData.Data.Calculate();
                ResultData.Data.Status = ResultData.Data.Status?.Trim();
                Summary = ResultData.Data;
                switch (Summary.Status)
                {
                    case "P":
                        Status = Resource.StatusP;
                        break;

                    case "A":
                        Status = Resource.StatusA;
                        break;

                    case "WE":
                        Status = Resource.StatusWE;
                        break;

                    case "H":
                        Status = Resource.StatusH;
                        break;

                    case "PE":
                        Status = Resource.StatusPE;
                        break;

                    case "UE":
                        Status = Resource.StatusUE;
                        break;

                    case "CM":
                        Status = Resource.StatusCM;
                        break;

                    case "JB":
                        Status = Resource.StatusJB;
                        break;

                    default:
                        Status = Resource.StatusUnknown;
                        break;
                }

                OxyModel = CreatePlotModel(Summary.GapDurationWithoutExcuse, Summary.WorkDuration, Summary.ShiftDuration);
                CancellAll();
                IsChartsVisible = true;
                FillChartsAnimation(Summary.TotalWorkingHoursPercent, Summary.TotalAbsentDaysPercent, Summary.TotalLateHoursPercent, _cancellationToken.Token);

                UpdateOxyplot?.Invoke();

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardResultViewModel : FillSummary");
            }
        }


        private PlotModel CreatePlotModel(double first, double second, double third)
        {
            try
            {
            first /= 3600;
            second /= 3600;
            third /= 3600;
            var max = Math.Max(first, Math.Max(second, third));

            max = Math.Abs(max) < 0.1 ? 1 : max * 1.1;

            return new PlotModel
            {
                Axes =
                {
                    new CategoryAxis
                    {
                        Position = AxisPosition.Bottom,
                        AbsoluteMinimum = -0.5,
                        AbsoluteMaximum = 2.5,
                        TextColor = OxyColors.Transparent,

                    },
                    new LinearAxis
                    {
                        Title = Resource.HoursText,
                        Position = AxisPosition.Left,
                        AbsoluteMinimum = 0,
                        AbsoluteMaximum = max
                    }
                },
                Series =
                {
                    new ColumnSeries
                    {
                        Items =
                        {
                            new ColumnItem
                            {
                                Value = first,
                                Color = OxyColors.Red
                            },
                            new ColumnItem
                            {
                                Value = second,
                                Color = OxyColors.Green
                            },
                            new ColumnItem
                            {
                                Value = third,
                                Color = OxyColors.Blue
                            },
                            new ColumnItem
                            {
                                Value = max,
                                Color = OxyColors.Transparent
                            }
                        }
                    }
                }
            };
            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "DashboardResultViewModel : CreatePlotModel");
                return new PlotModel();
            }
        }


        public void CancellAll()
        {
            _cancellationToken?.Cancel();
            _cancellationToken = new CancellationTokenSource();
        }
    }
}
