
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager _instance;
    public Transform _parent;

    public Dictionary<string, GameObject> _dictUIPanel = new Dictionary<string, GameObject>();
    public List<GameObject> _listPanel = new List<GameObject>();
    public Dictionary<string, GameObject> _acticeUI = new Dictionary<string, GameObject>();

    public GameObject _uiMess;
    public List<GameObject> _listMessage = new List<GameObject>();
    private void Awake()
    {
        _instance = this;
        for (int i = 0; i < _listPanel.Count; i++)
        {
            _dictUIPanel.Add(_listPanel[i].name, _listPanel[i]);
        }
    }

    public void BuidPanel<T>(string name)
    {
        GameObject go = null;
        _dictUIPanel.TryGetValue(name, out go);
        GameObject obj = Instantiate(go, _parent);
        obj.AddComponent(typeof(T));
        _acticeUI.Add(name, obj);
    }
    public void RecyclePanel(string name)
    {
        DestroyObject(_acticeUI[name]);
        _acticeUI.Remove(name);
    }

    public void RecycleAllPanel()
    {
        foreach (var item in _acticeUI)
        {
            RecyclePanel(item.Key);
        }
    }

    public void BuidPanelMess(string name, string mess)
    {
        if(_listMessage.Count > 9)
        {
            DestroyObject(_listMessage[0]);
            _listMessage.Remove(_listMessage[0]);
        }
        else
        {
            GameObject obj = Instantiate(_uiMess, _parent);
            _listMessage.Add(obj);
            obj.AddComponent<MessageLog>();
            obj.GetComponent<MessageLog>().SetMess(mess);
            
        }     
    }
    public void RecyclePanelMess(GameObject go)
    {
        DestroyObject(go);
        _listMessage.Remove(_listMessage.Find( x => x.gameObject == go));
    }
}

