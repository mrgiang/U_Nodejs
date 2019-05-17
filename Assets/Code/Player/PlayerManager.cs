using Project.Networking;
using Project.Utility.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private float speed = 4;

        [Header("Class references")]
        [SerializeField]
        [GreyOut]
        private NetworkIdentity networkIdentity;
        [SerializeField]
        [GreyOut]
        private JoyStickControl joyStick;
        [SerializeField]
        [GreyOut]
        private GetMotion motion;
        private void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            motion = GetComponent<GetMotion>();
            joyStick = JoyStickControl.instance;
        }
        public void Update()
        {
            if (networkIdentity.IsControlling())
            {
                checkMovement();
            }
        }

        private void checkMovement()
        {
            //float horizontal = Input.GetAxis("Horizontal");
            //float vertical = Input.GetAxis("Vertical");
            if (motion.animator.GetCurrentAnimatorStateInfo(0).IsName("stand") || motion.animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
            {
                if (joyStick.direction == Vector2.zero)
                {
                    motion.PlayAnimation("stand");
                }
                else
                {
                    motion.PlayAnimation("run");
                    transform.position += new Vector3(joyStick.direction.x, 0, joyStick.direction.y) * speed * Time.deltaTime;
                    transform.LookAt(new Vector3(transform.position.x + joyStick.direction.x,
                        transform.position.y,
                        transform.position.z + joyStick.direction.y));
                }
            }            
        }
    }
}
