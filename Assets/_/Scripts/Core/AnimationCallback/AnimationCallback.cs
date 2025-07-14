using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationCallback : MonoBehaviour
{
    protected AnimationComponent AnimationComponent;

    protected void Awake()
    {
        AnimationComponent = GetComponent<AnimationComponent>();
    }
}