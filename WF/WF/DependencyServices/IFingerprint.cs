using System;
using System.Collections.Generic;
using System.Text;

namespace WF.DependencyServices
{
  public  interface IFingerprint
    {
        void StartListen(Action success = null, Action failure = null);
        void StopListen();
    }
}
