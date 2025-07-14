using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Character
{
    private EnemiesController _enemiesController;

    public void SetEnemiesController(EnemiesController enemiesController)
    {
        _enemiesController = enemiesController;
    }

    protected override void Awake()
    {
        base.Awake();
        GetEntityComponent<HealthComponent>().OnDead += OnDeadCallback;
        GetEntityComponent<MovementToTargetComponent>().OnMeetTarget += OnMovedToPosition;
    }

    protected void OnDestroy()
    {
        GetEntityComponent<HealthComponent>().OnDead -= OnDeadCallback;
        GetEntityComponent<MovementToTargetComponent>().OnMeetTarget -= OnMovedToPosition;
    }

    public override void Initialize()
    {
        Stats stats = GetEntityComponent<StatsComponent>().GetStats<Stats>();
        GetEntityComponent<HealthComponent>().SetMaxHealth(stats.MaxHealth);

        GetEntityComponent<MovementToTargetComponent>().SetMoveSpeed(stats.MovementSpeed);
        GetEntityComponent<MovementToTargetComponent>().SetEnable(false);

        GetEntityComponent<MovementByPathComponent>().SetMoveSpeed(stats.MovementSpeed);
        GetEntityComponent<MovementByPathComponent>().SetEnable(true);

        GetEntityComponent<CombatComponent>().DisableAttack();
    }

    private void OnDeadCallback()
    {
        GetEntityComponent<MovementToTargetComponent>().SetEnable(false);
        GetEntityComponent<MovementByPathComponent>().SetEnable(false);

        GetEntityComponent<AnimationComponent>().PlayAnimation("Die");
        GetEntityComponent<MovementByPathComponent>().RemoveOnMeetTarget();

        Invoke(nameof(ReturnToPool), 2f);
    }

    private void ReturnToPool()
    {
        _enemiesController.ReturnToPool(this);
    }


    private void OnMovedToPosition()
    {
        GetEntityComponent<AnimationComponent>().SetFloat("movement", 0);
        GetEntityComponent<CombatComponent>().EnableAttack();
    }
}