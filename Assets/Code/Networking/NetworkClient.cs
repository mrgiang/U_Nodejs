using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;
using Project.Utility;


namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {
        [Header("Network Client")]
        public static NetworkClient _instance;
        [SerializeField]
        private Transform networkContainer;
        [SerializeField]
        private GameObject playerPrefab;
        [SerializeField]
        public static string ClientID { get; private set; }
        [SerializeField]
        public CameraFollow _Camera;
        [SerializeField]
        public Camera UICamera;
        private Dictionary<string, NetworkIdentity> serverObject;
        [Header("Data Client")]
        public JSONObject _dataLogin;
        public JSONObject _dataListRole;
        public JSONObject _dataRole;
        public string _sceneName;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            _instance = this;
            initialize();
            setupEvent();

            _Camera = Camera.main.GetComponent<CameraFollow>();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void initialize()
        {
            serverObject = new Dictionary<string, NetworkIdentity>();
        }
        private void setupEvent()
        {
            On(CmdManager.Connect, (E) =>
            {
                Debug.Log("connect to server");
                PanelManager._instance.BuidPanel<Login>(PanelName.Login);
            });
          
            On(CmdManager.Diconnected, (E) =>
            {
                string id = E.data["id"].ToString().RemoveQuotes();
                GameObject go = serverObject[id].gameObject;
                Destroy(go);
                serverObject.Remove(id);
            });
            On(CmdManager.Id, (E) => {
                Debug.Log(E);
                string id = E.data["id"].ToString().RemoveQuotes();
                ClientID = id;
                Debug.Log("It's ClientID:" + ClientID);
            });

            
               
            On(CmdManager.UpdatePosition, (E) =>
            {
                Debug.Log(E);
                string id = E.data["id"].ToString().RemoveQuotes();
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float z = E.data["position"]["z"].f;

                float q = E.data["rotation"].f;
                string motion = E.data["motion"].ToString().RemoveQuotes();
                NetworkIdentity ni = serverObject[id];
                ni.transform.position = new Vector3(x, y, z);
                ni.transform.localEulerAngles = new Vector3(0, q, 0);
                // ni.transform.GetComponents<GetMotion>().PlayAnimation(motion);
            });
        }

        private void SocketIOEventPawn(SocketIOEvent obj)
        {
            Debug.Log(obj);
            string id = obj.data["id"].ToString().RemoveQuotes();

            GameObject go = Instantiate(playerPrefab, networkContainer);
            go.name = playerPrefab.name;
            NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
            ni.SetControllerID(id);
            ni.SetSocketReference(this);
            serverObject.Add(id, ni);
            if (id == ClientID)
            {
                _Camera._myClient = ni.transform;
                PanelManager._instance.BuidPanel<JoyStickControl>(PanelName.Joystick);
            }
        }

        public void OnSocketIOEventPawn()
        {
            On(CmdManager.Pawn, SocketIOEventPawn);
        }
    }

    [Serializable]
    public class Player
    {
        public string id;
        public Position position;
        public float rotation;
        public string motion;
    }

    [Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }
}
