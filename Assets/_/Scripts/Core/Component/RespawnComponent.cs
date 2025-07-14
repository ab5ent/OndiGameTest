using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RespawnComponent : EntityComponent
{
    [SerializeField]
    private float respawnTime = -1;

    private float _respawnTimer = -1;

    public event Action OnRespawn;

    public void SetRespawnTime(float time)
    {
        respawnTime = time;
    }

    public void StartRespawnCooldown()
    {
        _respawnTimer = respawnTime;
    }

    public void Update()
    {
        if (_respawnTimer < 0)
        {
            return;
        }

        _respawnTimer -= Time.deltaTime;

        if (_respawnTimer <= 0)
        {
            OnRespawn?.Invoke();
        }
    }
}