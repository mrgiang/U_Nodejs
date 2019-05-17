using Project.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageLog : Mediator {
    [SerializeField]
    private UILabel uILabel;
    private int time = 2;
    private float stillCount = 0;
    private void Awake()
    {
        uILabel = transform.Find("Label").GetComponent<UILabel>();
    }
    public void SetMess(string mess)
    {
        uILabel.text = mess;
    }
    private void Update()
    {
        if(stillCount < 2)
        {
            stillCount += Time.deltaTime;
        }
        else
        {
            PanelManager._instance.RecyclePanelMess(this.gameObject);
        }
    }

    public override void onUpdate(string key, object para = null)
    {
        throw new System.NotImplementedException();
    }
}
