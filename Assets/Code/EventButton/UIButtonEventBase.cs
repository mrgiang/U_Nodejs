using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WTManager.UIManager.Base
{
    public abstract class UIButtonEventBase
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
        public abstract void DoClick();
    }
}
