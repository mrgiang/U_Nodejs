using Project.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WTManager.UIManager.Base;

public class SelectRole : Mediator
{
    [Header("References Login")]
    [SerializeField]
    private SelectRole_LoginExcute _buttonRole1;
    private UIButton _uiRole1;
    [SerializeField]
    private SelectRole_LoginExcute _buttonRole2;
    private UIButton _uiRole2;
    [SerializeField]
    private SelectRole_LoginExcute _buttonRole3;
    private UIButton _uiRole3;
    [SerializeField]
    private SelectRole_RegisExcute _buttonRegis;
    private UIButton _uiRegis;
    [SerializeField]
    private SelectRole_BackExcute _buttonBack;

    [Header("Networking")]
    private NetworkClient _networkClient;
    [Header("Constructor")]
    private int _roleSelect = 0;
    private List<UIButton> _listUIbutton = new List<UIButton>();
    void Start()
    {
        _networkClient = NetworkClient._instance;
        _uiRole1 = transform.Find("SelectRole-role1").GetComponent<UIButton>();
        _uiRole1.isEnabled = false;
        _uiRole2 = transform.Find("SelectRole-role2").GetComponent<UIButton>();
        _uiRole2.isEnabled = false;
        _uiRole3 = transform.Find("SelectRole-role3").GetComponent<UIButton>();
        _uiRole3.isEnabled = false;
        _listUIbutton.Add(_uiRole1);
        _listUIbutton.Add(_uiRole2);
        _listUIbutton.Add(_uiRole3);

        _uiRegis = transform.Find("SelectRole-Regis").GetComponent<UIButton>();

        _buttonRole1 = EventCenter.RegisterButtonEvent("SelectRole-role1", new SelectRole_LoginExcute()) as SelectRole_LoginExcute;
        _buttonRole1.data.Add("1", 0);
        _buttonRole2 = EventCenter.RegisterButtonEvent("SelectRole-role2", new SelectRole_LoginExcute()) as SelectRole_LoginExcute;
        _buttonRole2.data.Add("1", 1);
        _buttonRole3 = EventCenter.RegisterButtonEvent("SelectRole-role3", new SelectRole_LoginExcute()) as SelectRole_LoginExcute;
        _buttonRole3.data.Add("1", 2);

        _buttonRegis = EventCenter.RegisterButtonEvent("SelectRole-Regis", new SelectRole_RegisExcute()) as SelectRole_RegisExcute;
        _buttonBack = EventCenter.RegisterButtonEvent("SelectRole-Back", new SelectRole_BackExcute()) as SelectRole_BackExcute;
        Show();
    }
    private void Show()
    {
        if (_networkClient._dataListRole.Count >= 3)
        {
            _uiRegis.isEnabled = false;
        }
        for (int i = 0; i < _networkClient._dataListRole.Count; i++)
        {
            DataRole(_listUIbutton[i]);
        }
    }
    private void DataRole(UIButton button)
    {
        button.isEnabled = true;
    }
    private void OnClickRole(int par)
    {
        _roleSelect = par;
        _networkClient._dataRole = _networkClient._dataListRole[_roleSelect];
        _networkClient.Emit(CmdManager.LogGame, _networkClient._dataRole);
        Debug.Log("Loggame role " + _networkClient._dataRole);
        _networkClient._sceneName = MapName.BaLangHuyen;

        PanelManager._instance.RecyclePanel(PanelName.SelectRole);
        PanelManager._instance.BuidPanel<ProgressSceneLoader>(PanelName.LoadScene);
        _networkClient.OnSocketIOEventPawn();
    }

    private void OnClickRegis()
    {
        PanelManager._instance.RecyclePanel(PanelName.SelectRole);
        PanelManager._instance.BuidPanel<RegisRole>(PanelName.RegisRole);
    }

    private void OnClickBack()
    {
        PanelManager._instance.RecyclePanel(PanelName.SelectRole);
        _networkClient.Emit(CmdManager.ReLogin);
        PanelManager._instance.BuidPanel<Login>(PanelName.Login);
    }

    public override void onUpdate(string key, object para = null)
    {
        switch (key)
        {
            case "OnClickRole": OnClickRole((int)para); break;
            case "OnClickRegis": OnClickRegis(); break;
            case "OnClickBack": OnClickBack(); break;
        }
    }
    private void OnDestroy()
    {
        _uiRole1 = null;
        _uiRole2 = null;
        _uiRole3 = null;
        EventCenter.UnRegisterButtonEvent("SelectRole-role1");
        EventCenter.UnRegisterButtonEvent("SelectRole-role2");
        EventCenter.UnRegisterButtonEvent("SelectRole-role3");
        EventCenter.UnRegisterButtonEvent("SelectRole-Regis");
        EventCenter.UnRegisterButtonEvent("SelectRole-Back");

        _buttonRole1 = null;
        _buttonRole2 = null;
        _buttonRole3 = null;
        _buttonRegis = null;
        _buttonBack = null;

        _listUIbutton = null;
        _networkClient = null;
    }

}
public class SelectRole_LoginExcute : UIButtonEventBase
{
    public override void DoClick()
    {
        PanelManager._instance._acticeUI[PanelName.SelectRole].GetComponent<Mediator>().onUpdate("OnClickRole", data["1"]);
    }
}

public class SelectRole_RegisExcute : UIButtonEventBase
{
    public override void DoClick()
    {
        PanelManager._instance._acticeUI[PanelName.SelectRole].GetComponent<Mediator>().onUpdate("OnClickRegis");
    }
}

public class SelectRole_BackExcute : UIButtonEventBase
{
    public override void DoClick()
    {
        PanelManager._instance._acticeUI[PanelName.SelectRole].GetComponent<Mediator>().onUpdate("OnClickBack");
    }
}
