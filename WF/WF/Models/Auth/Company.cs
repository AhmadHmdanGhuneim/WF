using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Models.Auth
{
   public class CompanyLiscence
    {
        public string cmpid { get; set; }

        public string cmpname { get; set; }
        public string cmpUrl { get; set; }

        public Guid Token { get; set; }

    }
}
