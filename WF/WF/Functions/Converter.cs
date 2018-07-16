using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WF.Models.Auth;

namespace WF.Functions
{
   public static class Converter
    {

        public static User ConvetToUser(string prmResponeBody)
        {
            try
            {
                User user = new User();
                JObject jObject = JObject.Parse(prmResponeBody);
                //user.ResultCode = Convert.ToInt32(jObject["resultCode"].ToString());
                user.FullName = jObject["fullName"].ToString();
                user.IsGregorianLocale = Convert.ToBoolean(jObject["isGregorianLocale"].ToString());
                user.IsManager = Convert.ToBoolean(jObject["isManager"].ToString());
                user.EmpId = jObject["empId"].ToString();


                return user;

            }
            catch (Exception exception)
            {
                GeneralFunctions.HandelException(exception, "ConvertToUser");
                return null;
            }
        }


    }
}
