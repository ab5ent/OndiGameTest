using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatComponent : EntityComponent
{
    [SerializeField] private CircleCollider2D rangeCollider;

    [SerializeField] private BaseAttack[] attacks;

    [SerializeField] private Entity target;

    [SerializeField] private List<Entity> inRangeTargets;

    private bool _isAttacking, _canAttack;

    private int _attackRange;

    public event Action OnEnemyKilled;

    public override void Initialize(Entity owner)
    {
        base.Initialize(owner);

        foreach (BaseAttack attack in attacks)
        {
            attack.Initialize(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<HitboxComponent>(out var hitbox))
        {
            return;
        }

        HealthComponent healthComponent = hitbox.Owner.GetEntityComponent<HealthComponent>();

        if (!healthComponent || healthComponent.IsDead())
        {
            return;
        }

        inRangeTargets.Add(hitbox.Owner);

        if (target == null && _canAttack)
        {
            SetAttacking(hitbox.Owner);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<HitboxComponent>(out var hitbox))
        {
            return;
        }

        inRangeTargets.Remove(hitbox.Owner);

        if (target == hitbox.Owner)
        {
            SetIdle();
        }
    }

    private void OnDrawGizmos()
    {
        if (rangeCollider == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeCollider.radius);
    }

    private void Update()
    {
        if (target == null && _canAttack)
        {
            TryToGetNearestTarget();
        }
    }

    private void TryToGetNearestTarget()
    {
        if (inRangeTargets.Count == 0)
        {
            SetIdle();
            return;
        }

        Entity nearestTarget = GetNearestTarget();

        if (nearestTarget != null)
        {
            SetAttacking(nearestTarget);
        }
        else
        {
            SetIdle();
        }
    }

    private void SetAttacking(Entity newTarget)
    {
        target = newTarget;
        _isAttacking = true;
        _attackRange = Vector2.Distance(transform.position, target.transform.position) <= 1 ? 0 : 1;

        Owner.GetEntityComponent<AnimationComponent>().SetBool("isAttacking", _isAttacking);
        Owner.GetEntityComponent<AnimationComponent>().SetFloat("attackRange", _attackRange);
        Owner.GetEntityComponent<RendererComponent>().SetRotate(transform.position - target.transform.position);
    }

    public void SetIdle()
    {
        _isAttacking = false;
        _attackRange = -1;
        target = null;
        Owner.GetEntityComponent<AnimationComponent>().SetBool("isAttacking", _isAttacking);
    }

    public void DisableAttack()
    {
        _attackRange = -1;
        _isAttacking = false;
        _canAttack = false;
        target = null;

        Owner.GetEntityComponent<AnimationComponent>().SetBool("isAttacking", _isAttacking);
    }

    public void EnableAttack()
    {
        _attackRange = -1;
        _canAttack = true;
        _isAttacking = false;
        target = null;
    }

    public void OnAttack(string animationName)
    {
        foreach (var attack in attacks)
        {
            if (attack.AnimationName != animationName)
            {
                continue;
            }

            attack.Attack(Owner, target);
            return;
        }
    }

    public void OnAttack(string animationName, Transform projectileSpawnPoint)
    {
        foreach (var attack in attacks)
        {
            if (attack.AnimationName != animationName)
            {
                continue;
            }

            attack.Attack(Owner, target, projectileSpawnPoint);
            return;
        }
    }

    public void TriggerOnEnemyKilled()
    {
        if (target != null)
        {
            inRangeTargets.Remove(target);
            target = null;
        }

        OnEnemyKilled?.Invoke();
    }

    public bool HasTargets()
    {
        return inRangeTargets.Count != 0;
    }

    public Entity GetCurrentTarget()
    {
        return target;
    }

    public Entity GetNearestTarget()
    {
        Entity nearestTarget = null;
        float nearestDistance = float.MaxValue;
        Vector2 currentPos = transform.position;

        foreach (var potentialTarget in inRangeTargets)
        {
            if (potentialTarget == null) continue;

            float distance = Vector2.Distance(currentPos, potentialTarget.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = potentialTarget;
            }
        }

        return nearestTarget;
    }

    public void TriggerOnKillAEnemy(Entity aTarget)
    {
        if (inRangeTargets.Contains(aTarget))
        {
            inRangeTargets.Remove(aTarget);
        }

        OnEnemyKilled?.Invoke();
    }
}