using Project.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MotionType
{
    attack01,
    attack02,
    die,
    fly01,
    hurt,
    hurt01,
    hurt02,
    run,
    stand,
    stun
}
public class GetMotion : MonoBehaviour
{
    //public bool isMy;
    public Animator animator;
    //private int indexClip = 0;
    private Dictionary<string, float> dictCliplength = new Dictionary<string, float>();
    //private float frameTime = 0;
    private MotionType motionType = MotionType.stand;
    //private int layerIndex = 0;
    private NetworkTransform networkTransform;
    //public int Idplayer ;
    void Start()
    {
        animator = GetComponent<Animator>();
        networkTransform = GetComponent<NetworkTransform>();
        Init(animator);
    }

    private void Init(Animator _animator)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = _animator.runtimeAnimatorController;

        foreach (AnimationClip aClip in _animator.runtimeAnimatorController.animationClips)
        {
            overrideController[aClip.name] = GetResources.GetAnimationClip(transform.name, aClip.name);
            dictCliplength.Add(aClip.name, overrideController[aClip.name].length);
        }
        _animator.runtimeAnimatorController = overrideController;
    }

    //private void Update()
    //{

    //    //if (!isMy) return;
    //    if (frameTime > 0 && Time.time >= frameTime)
    //    {
    //        frameTime = 0;
    //        Debug.Log("cast skill");
    //        switch (motionType)
    //        {
    //            case MotionType.attack01:
    //                break;
    //            case MotionType.attack02: break;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.Q) && animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(MotionType.stand.ToString()))
    //    {
    //        PlayAnimation(MotionType.attack01.ToString());
    //       // animator.GetCurrentAnimatorClipInfo(0)[0].clip.name
    //    }
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        PlayPassiveAnimation(MotionType.hurt.ToString());
    //    }
    //    if (Input.GetKeyDown(KeyCode.N) && animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(MotionType.stand.ToString()))
    //    {
    //        PlayAnimation(((MotionType)indexClip).ToString());
    //        indexClip++;
    //    }
    //}
    public void PlayAnimation(string name)// chủ động gọi
    {
        animator.Play(name);
        networkTransform.player.motion = name;
        //motionType = (MotionType)Enum.Parse(typeof(MotionType), name);
        //float length = 0;
        //dictCliplength.TryGetValue(name, out length);
        //frameTime = length + Time.time;
    }
    public void PlayPassiveAnimation(string name)// bị động gọi
    {
        //frameTime = 0;
        animator.Play(name);
        networkTransform.player.motion = name;
        //motionType = (MotionType)Enum.Parse(typeof(MotionType), name);
    }


}
