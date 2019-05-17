using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WTManager.UIManager.Base;

namespace WTManager.UIManager.Base
{
    [RequireComponent(typeof(UIButton))]
    public class UIButtonBase : MonoBehaviour
    {
        private UIButton thisButton;
        public List<EventDelegate> EventHandler
        {
            get
            {
                return thisButton.onClick;
            }
        }

        private void Awake()
        {
            thisButton = GetComponent<UIButton>();
            thisButton.onClick = EventHandler;
            EventHandler.Clear();
            EventDelegate eDelegate = new EventDelegate(this, "ExcuteMethod");
            EventHandler.Add(eDelegate);
        }

        private void ExcuteMethod() {
            UIButtonEventBase buttonEventBase = EventCenter.GetButtonEvent(gameObject.name);

            if (buttonEventBase != null)
            {
                buttonEventBase.DoClick();
            }
        }

    }
}
