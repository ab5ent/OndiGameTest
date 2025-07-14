using System;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] protected float time;

    [SerializeField] protected float height;

    public virtual void Launch(Transform startPoint, Transform endPoint, Action OnHit)
    {

    }
}