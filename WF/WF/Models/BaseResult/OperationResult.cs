using System;
using System.Collections.Generic;
using System.Text;

namespace WF.Models.BaseResult
{
   public class OperationResult
    {
        public ResultCode ResultCode { get; set; }

        public static OperationResult Result(ResultCode code)
        {
            var op = new OperationResult
            {
                ResultCode = code
            };
            return op;
        }
    }
}
