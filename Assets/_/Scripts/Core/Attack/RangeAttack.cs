using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : BaseAttack
{
    [SerializeField] private SimpleProjectile projectilePrefab;

    [SerializeField] private int initialPoolSize = 15;

    private readonly Queue<SimpleProjectile> _projectilePool = new Queue<SimpleProjectile>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewProjectile();
        }
    }

    private SimpleProjectile CreateNewProjectile()
    {
        SimpleProjectile projectile = Instantiate(projectilePrefab, transform);
        projectile.gameObject.SetActive(false);
        _projectilePool.Enqueue(projectile);
        return projectile;
    }

    private SimpleProjectile GetProjectile()
    {
        if (_projectilePool.Count == 0)
        {
            return CreateNewProjectile();
        }

        var projectile = _projectilePool.Dequeue();
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    private void ReturnToPool(SimpleProjectile projectile)
    {
        if (projectile != null)
        {
            projectile.gameObject.SetActive(false);
            projectile.transform.SetParent(transform);
            _projectilePool.Enqueue(projectile);
        }
    }

    public override void Attack(Entity owner, Entity target, Transform projectileSpawnPoint)
    {
        SimpleProjectile projectile = GetProjectile();
        projectile.transform.SetParent(null);
        projectile.transform.SetPositionAndRotation(projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Pass a callback to return the projectile to the pool when it's done
        projectile.Launch(projectileSpawnPoint, target.transform, () =>
        {
            Attack(owner, target);
            ReturnToPool(projectile);
        });
    }

    public override void Attack(Entity owner, Entity target)
    {
        HealthComponent targetHealth = target.GetEntityComponent<HealthComponent>();
        Stats targetStats = target.GetEntityComponent<StatsComponent>().GetStats<Stats>();

        Stats ownerStats = owner.GetEntityComponent<StatsComponent>().GetStats<Stats>();

        targetHealth.ChangeCurrentHealth(-Mathf.Max(0, ownerStats.Damage - targetStats.Armor));

        if (targetHealth.IsDead())
        {
            _combatComponent.TriggerOnEnemyKilled();
            _combatComponent.SetIdle();
        }
    }
}