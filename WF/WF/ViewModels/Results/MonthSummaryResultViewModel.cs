using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WF.Helpers;
using WF.Models.Summary;
using Xamarin.Forms;

namespace WF.ViewModels.Results
{
  public  class MonthSummaryResultViewModel : ObservableObject
    {

        public ObservableCollection<AttendanceList> Summaries { get; set; } = new ObservableCollection<AttendanceList>();
        public SelectSummary SelectSummary { get; set; } = new SelectSummary();

        public TextAlignment UniversalHorizontalTextAligment => IsLtrLang ? TextAlignment.End : TextAlignment.Start;

        // if is english set postion Make first Parameter else make secand parameter
       







        public MonthSummaryResultViewModel(ObservableCollection<AttendanceList> attendanceLists, SelectSummary selectSummary)
        {
            Summaries = attendanceLists;
            SelectSummary = selectSummary;
        }
    


    }
}
