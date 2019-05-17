using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _myClient { get; set; }
    private Vector3 _offsetPos;
    private Quaternion _offsetRot;
    private void Start()
    {
        _offsetPos = transform.position;
        _offsetRot = transform.rotation;
    }

    private void Update()
    {
        if (_myClient == null) return;
        transform.position = new Vector3(_offsetPos.x + _myClient.position.x,
                                         _offsetPos.y + _myClient.position.y,
                                         _offsetPos.z + _myClient.position.z);
        //transform.LookAt(_myClient);
    }
}
