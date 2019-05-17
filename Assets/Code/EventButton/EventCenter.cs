using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WTManager.UIManager.Base
{
    public static class EventCenter
    {
        private static Dictionary<string, object> eventLibrary;
        public static Dictionary<string, object> EventLibrary
        {
            get
            {
                if (eventLibrary == null)
                {
                    eventLibrary = new Dictionary<string, object>();
                }
                return eventLibrary;
            }
        }

        public static UIButtonEventBase RegisterButtonEvent(string buttonName, UIButtonEventBase buttonEvent)
        {
            if (EventLibrary.ContainsKey(buttonName))
            {
                EventLibrary[buttonName] = buttonEvent;
            }
            else
            {
                EventLibrary.Add(buttonName, buttonEvent);
            }
            return GetButtonEvent(buttonName);
        }

        public static UIButtonEventBase GetButtonEvent(string buttonName)
        {
            return EventLibrary[buttonName] as UIButtonEventBase;
        }

        public static void UnRegisterButtonEvent(UIButtonBase button)
        {
            if (EventLibrary.ContainsKey(button.name))
            {
                EventLibrary[button.name] = null;
            }
        }
        public static void UnRegisterButtonEvent(string buttonName)
        {
            if (EventLibrary.ContainsKey(buttonName))
            {
                EventLibrary[buttonName] = null;
            }
        }
    }

}
