using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Models.Summary
{
   public class SelectSummary : WF.Helpers.ObservableObject
    {

        public string Department { get; set; }
        public string Employee { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }


        public string ReqestType { get; set; }

        public string RequetStatus { get; set; }


        public string FromDate { get; set; }

        public string ToDate { get; set; }

        

    }
}
