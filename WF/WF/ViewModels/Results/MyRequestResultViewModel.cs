using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WF.Helpers;
using WF.Models.Reports;
using WF.Models.Summary;

namespace WF.ViewModels.Results
{
   public class MyRequestResultViewModel : ObservableObject
    {

        public ObservableCollection<ReportRequest> Requests { get; set; } = new ObservableCollection<ReportRequest>();

        public SelectSummary SelectSummary { get; set; } = new SelectSummary();


        public int DatePostion => IsLtrLang ? 0 : 3;
        public int RequsetPostion => IsLtrLang ? 1 : 2;
        public int TypePosition => IsLtrLang ? 2 : 1;
        public int StatusPosition => IsLtrLang ? 3 : 0;


        public MyRequestResultViewModel(ObservableCollection<ReportRequest> prmRequests,SelectSummary prmSelectdSummary)
        {
            Requests = prmRequests;
            SelectSummary = prmSelectdSummary;
        }

    }
}
