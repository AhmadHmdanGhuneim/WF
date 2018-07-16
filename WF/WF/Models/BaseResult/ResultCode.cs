using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Models.BaseResult
{
    public enum ResultCode
    {

        Cancelled = -1,

        UnknownError = 0,

        Success = 1,

        NoAccess = 2,

        AuthError = 3,

        AuthTokenError = 4,

        //muath dassan28-11-2017
        ///////////////////////////////////////////
        Exc_MaxDays = 5,
        Exc_MaxPercentage = 6,
        Vac_NoWork = 7,
        Vac_DaysLimit = 8,
        Vac_HaveTrans = 9,
        Vac_Nesting = 10,
        Vac_NoEnterReset = 11,
        Vac_MaxDays = 12
        ///////////////////////////////////////////
    }
}
