using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Models.Auth
{
  public  class User
    {
       // public int ResultCode { get; set; }

        public string EmpId { get; set; }

        public string Login { get; set; }

        public string Token { get; set; }

        public string FullName { get; set; }

        public bool IsManager { get; set; }

        public bool IsGregorianLocale { get; set; }
    }
}
