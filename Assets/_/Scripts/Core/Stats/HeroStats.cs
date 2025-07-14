using System;
using UnityEngine;

[Serializable]
public class HeroStats : Stats
{
    [field: SerializeField]
    public float RespawnTime { get; private set; }
}