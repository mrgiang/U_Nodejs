using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ResourcesType
{
    AnimationClip
}
public class GetResources
{
    public static AnimationClip GetAnimationClip(string name, string nameClip)
    {
      return  Resources.Load<AnimationClip>("Animations/" + name + "/Animation/" + nameClip);
    }
}
