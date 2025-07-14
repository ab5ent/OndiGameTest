using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComponent : EntityComponent
{
    [SerializeField] private int currentLevel = 0;

    public event Action<int> OnLevelUp;

    [ContextMenu("Level Up")]
    public void LevelUp()
    {
        currentLevel++;
        OnLevelUp?.Invoke(currentLevel);
    }
}