using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseComponent : EntityComponent
{
    [SerializeField] private CircleCollider2D _circle;

    private Entity _target;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<HitboxComponent>(out var hitbox))
        {
            return;
        }

        HealthComponent healthComponent = hitbox.Owner.GetEntityComponent<HealthComponent>();

        HealthComponent ownerHealthComponent = Owner.GetEntityComponent<HealthComponent>();

        if (!healthComponent || healthComponent.IsDead() || ownerHealthComponent.IsDead())
        {
            return;
        }

        _target = hitbox.Owner;

        Owner.GetEntityComponent<MovementByPathComponent>().SetEnable(false);
        Owner.GetEntityComponent<MovementByPathComponent>().SetMoving(false);

        Owner.GetEntityComponent<MovementToTargetComponent>().SetEnable(true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<HitboxComponent>(out var hitbox))
        {
            return;
        }

        if (hitbox.Owner == _target)
        {
            _target = null;
        }

        Owner.GetEntityComponent<MovementByPathComponent>().SetEnable(true);
        Owner.GetEntityComponent<MovementByPathComponent>().MoveToClosestPathPoint();

        Owner.GetEntityComponent<MovementToTargetComponent>().StopMoving();
        Owner.GetEntityComponent<MovementToTargetComponent>().SetEnable(false);
    }

    private void Update()
    {
        if (_target != null)
        {
            Owner.GetEntityComponent<MovementToTargetComponent>().MoveToPosition(_target.transform.position);
            Owner.GetEntityComponent<RendererComponent>().SetRotate(transform.position - _target.transform.position);
        }
    }
}