using System;
using System.Collections.Generic;
using System.Text;

namespace WF.DependencyServices
{
  public interface IPushRegister
    {
        void Register(string tag);

        void Unregister();
    }
}
