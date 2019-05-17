using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mediator : MonoBehaviour
{
    public abstract void onUpdate(string key, object para = null);
}
