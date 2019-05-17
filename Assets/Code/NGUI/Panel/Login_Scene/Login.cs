using Project.Networking;
using Project.Utility;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WTManager.UIManager.Base;

public class Login : Mediator
{
    [Header("References Login")]
    [SerializeField]
    private Login_LoginExcute _buttonLogin;
    [SerializeField]
    private Login_RegisterExcute _buttonRegister;
    [SerializeField]
    private UIInput _inputUsername;
    [SerializeField]
    private UIInput _inputPassword;
    [Header("Networking")]
    private NetworkClient _networkClient;
    private void Start()
    {
        _networkClient = NetworkClient._instance;

        _inputUsername = transform.Find("Control - Simple Input Field - Username").GetComponent<UIInput>();
        _inputPassword = transform.Find("Control - Simple Input Field - Password").GetComponent<UIInput>();

        _buttonLogin = EventCenter.RegisterButtonEvent("Login-Login", new Login_LoginExcute()) as Login_LoginExcute;
        _buttonRegister = EventCenter.RegisterButtonEvent("Login-Register", new Login_RegisterExcute()) as Login_RegisterExcute;

        _networkClient.On(CmdManager.Login, SocketIOLogin);
        _networkClient.On(CmdManager.Register, SocketIORegister);
        _networkClient.On(CmdManager.GetRole, SocketIOGetRole);       
    }

    private void SocketIORegister(SocketIOEvent obj)
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
            JSONObject json = new JSONObject();
            json.AddField("username", obj.data["item"]["username"].ToString().RemoveQuotes());
            json.AddField("password", obj.data["item"]["password"].ToString().RemoveQuotes());
            _networkClient.Emit(CmdManager.Login, json);
        }
    }

    private void SocketIOGetRole(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        bool err = obj.data["Err"].b;
        PanelManager._instance.RecyclePanel(PanelName.Login);
        if (err)
        {
            string mess = obj.data["mess"].ToString().RemoveQuotes();
            PanelManager._instance.BuidPanelMess(PanelName.Message, mess);
            PanelManager._instance.BuidPanel<RegisRole>(PanelName.RegisRole);
        }
        else
        {
            _networkClient._dataListRole = obj.data["item"];
            PanelManager._instance.BuidPanel<SelectRole>(PanelName.SelectRole);
        }
    }

    private void SocketIOLogin(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        bool err = obj.data["Err"].b;
        if (err)
        {
            string mess = obj.data["mess"].ToString().RemoveQuotes();
            PanelManager._instance.BuidPanelMess(PanelName.Message , mess);
        }
        else
        {
            _networkClient._dataLogin = obj.data["item"];
            _networkClient.Emit(CmdManager.GetRole);
        }
    }
    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(_inputUsername.value) || string.IsNullOrEmpty(_inputPassword.value))
        {
            Debug.Log("username or password  is NullOrEmty");
        }
        else
        {
            JSONObject json = new JSONObject();
            json.AddField("username", _inputUsername.value);
            json.AddField("password", _inputPassword.value);
            _networkClient.Emit(CmdManager.Login, json);
            //_buttonLogin.enabled = false;
            //_buttonCancel.enabled = false;
        }
    }

    public void OnClickRegister()
    {
        if (string.IsNullOrEmpty(_inputUsername.value) || string.IsNullOrEmpty(_inputPassword.value))
        {
            Debug.Log("username or password  is NullOrEmty");
        }
        else
        {
            JSONObject json = new JSONObject();
            json.AddField("username", _inputUsername.value);
            json.AddField("password", _inputPassword.value);
            _networkClient.Emit(CmdManager.Register, json);
            //_buttonLogin.enabled = false;
            //_buttonCancel.enabled = false;
        }
    }


    private void OnDestroy()
    {
        _inputUsername = null;
        _inputPassword = null;
        EventCenter.UnRegisterButtonEvent("Login-Login");
        EventCenter.UnRegisterButtonEvent("Login-Register");
        _buttonLogin = null;
        _buttonRegister = null;
        _networkClient.Off(CmdManager.Login, SocketIOLogin);
        _networkClient.Off(CmdManager.Register, SocketIORegister);
        _networkClient.Off(CmdManager.GetRole, SocketIOGetRole);
        _networkClient = null;
    }

    public override void onUpdate(string key, object para)
    {
        switch (key)
        {
            case "OnClickLogin": OnClickLogin(); break;
            case "OnClickRegister": OnClickRegister(); break;
        }
    }

}

public class Login_LoginExcute : UIButtonEventBase
{

    public override void DoClick()
    {
        //data[""]
        PanelManager._instance._acticeUI[PanelName.Login].GetComponent<Mediator>().onUpdate("OnClickLogin");
    }
}
public class Login_RegisterExcute : UIButtonEventBase
{

    public override void DoClick()
    {
        //data[""]
        PanelManager._instance._acticeUI[PanelName.Login].GetComponent<Mediator>().onUpdate("OnClickRegister");
    }
}

