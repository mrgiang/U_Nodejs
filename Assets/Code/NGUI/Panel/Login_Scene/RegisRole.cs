using Project.Networking;
using Project.Utility;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTManager.UIManager.Base;

public class RegisRole : Mediator
{
    [Header("References RegisRole")]
    [SerializeField]
    private UIInput _inputNameRole;
    [SerializeField]
    private RegisRole_RoleExcute _buttonRole1;
    [SerializeField]
    private RegisRole_RoleExcute _buttonRole2;
    [SerializeField]
    private RegisRole_RoleExcute _buttonRole3;
    [SerializeField]
    private RegisRole_RoleExcute _buttonRole4;
    [SerializeField]
    private RegisRole_RoleExcute _buttonRole5;
    [SerializeField]
    private RegisRole_LoginExcute _buttonLogRole;
    [SerializeField]
    private RegisRole_BackExcute _buttonBack;
    [Header("Networking")]
    private NetworkClient _networkClient;
    [Header("Constructor")]
    private int _roleSelect = 0;
    private void Start()
    {
        _networkClient = NetworkClient._instance;
        _inputNameRole = transform.Find("Control - Simple Input Field").GetComponent<UIInput>();
        _buttonRole1 = EventCenter.RegisterButtonEvent("RegisRole-role1", new RegisRole_RoleExcute()) as RegisRole_RoleExcute;
        _buttonRole1.data.Add("id", 1);
        _buttonRole2 = EventCenter.RegisterButtonEvent("RegisRole-role2", new RegisRole_RoleExcute()) as RegisRole_RoleExcute;
        _buttonRole2.data.Add("id", 2);
        _buttonRole3 = EventCenter.RegisterButtonEvent("RegisRole-role3", new RegisRole_RoleExcute()) as RegisRole_RoleExcute;
        _buttonRole3.data.Add("id", 3);
        _buttonRole4 = EventCenter.RegisterButtonEvent("RegisRole-role4", new RegisRole_RoleExcute()) as RegisRole_RoleExcute;
        _buttonRole4.data.Add("id", 4);
        _buttonRole5 = EventCenter.RegisterButtonEvent("RegisRole-role5", new RegisRole_RoleExcute()) as RegisRole_RoleExcute;
        _buttonRole5.data.Add("id", 5);

        _buttonLogRole = EventCenter.RegisterButtonEvent("RegisRole-Login", new RegisRole_LoginExcute()) as RegisRole_LoginExcute;
        _buttonBack = EventCenter.RegisterButtonEvent("RegisRole-Back", new RegisRole_BackExcute()) as RegisRole_BackExcute;

        OnClickRole(1);

        _networkClient.On(CmdManager.RegisRole, SocketIORegisRole);
        _networkClient.On(CmdManager.GetRole, SocketIOGetRole);
    }

    private void SocketIORegisRole(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        bool err = obj.data["Err"].b;
        if (err)
        {
            string mess = obj.data["mess"].ToString().RemoveQuotes();
            PanelManager._instance.BuidPanelMess(PanelName.Message, mess);
        }
        else
        {
            _networkClient.Emit(CmdManager.GetRole);
        }
    }
    private void SocketIOGetRole(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        bool err = obj.data["Err"].b;
        if (err)
        {
            string mess = obj.data["mess"].ToString().RemoveQuotes();
            PanelManager._instance.BuidPanelMess(PanelName.Message, mess);
        }
        else
        {
            _networkClient._dataListRole = obj.data["item"];
            PanelManager._instance.RecyclePanel(PanelName.RegisRole);
            PanelManager._instance.BuidPanel<SelectRole>(PanelName.SelectRole);
        }
    }

    private void OnClickLogin()
    {
        Debug.Log("OnClickLogin ");
        if (string.IsNullOrEmpty(_inputNameRole.value))
        {
            Debug.Log("name role is NullOrEmty");
        }
        else
        {
            JSONObject json = new JSONObject();
            json.AddField("namerole", _inputNameRole.value);
            json.AddField("kind", _roleSelect);
            _networkClient.Emit(CmdManager.RegisRole, json);
        }
    }
    private void OnClickRole(int par)
    {
        _roleSelect = par;
        Debug.Log("Chọn role " + _roleSelect);
    }
    private void OnClickBack()
    {
        PanelManager._instance.RecyclePanel(PanelName.RegisRole);
        _networkClient.Emit(CmdManager.ReLogin);
        PanelManager._instance.BuidPanel<Login>(PanelName.Login);
    }
    private void OnDestroy()
    {
        _inputNameRole = null;
        EventCenter.UnRegisterButtonEvent("RegisRole-role1");
        EventCenter.UnRegisterButtonEvent("RegisRole-role2");
        EventCenter.UnRegisterButtonEvent("RegisRole-role3");
        EventCenter.UnRegisterButtonEvent("RegisRole-role4");
        EventCenter.UnRegisterButtonEvent("RegisRole-role5");
        EventCenter.UnRegisterButtonEvent("RegisRole-Login");
        EventCenter.UnRegisterButtonEvent("RegisRole-Back");
        _buttonRole1 = null;
        _buttonRole2 = null;
        _buttonRole3 = null;
        _buttonRole4 = null;
        _buttonRole5 = null;
        _buttonLogRole = null;
        _buttonBack = null;
        _networkClient.Off(CmdManager.RegisRole, SocketIORegisRole);
        _networkClient.Off(CmdManager.GetRole, SocketIOGetRole);
        _networkClient = null;
    }

    public override void onUpdate(string key, object para)
    {
        switch (key)
        {
            case "OnClickRole": OnClickRole((int)para); break;
            case "OnClickLogin": OnClickLogin(); break;
            case "OnClickBack": OnClickBack(); break;
        }
    }
}

public class RegisRole_RoleExcute : UIButtonEventBase
{

    public override void DoClick()
    {
        PanelManager._instance._acticeUI[PanelName.RegisRole].GetComponent<Mediator>().onUpdate("OnClickRole", data["id"]);
    }
}


public class RegisRole_LoginExcute : UIButtonEventBase
{

    public override void DoClick()
    {
        PanelManager._instance._acticeUI[PanelName.RegisRole].GetComponent<Mediator>().onUpdate("OnClickLogin");
    }
}
public class RegisRole_BackExcute : UIButtonEventBase
{

    public override void DoClick()
    {
        //data[""]
        PanelManager._instance._acticeUI[PanelName.RegisRole].GetComponent<Mediator>().onUpdate("OnClickBack");
    }
}

