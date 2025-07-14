using UnityEngine;

public class MeleeAttack : BaseAttack
{
    public override void Attack(Entity owner, Entity target)
    {
        HealthComponent targetHealth = target.GetEntityComponent<HealthComponent>();
        Stats targetStats = target.GetEntityComponent<StatsComponent>().GetStats<Stats>();

        Stats ownerStats = owner.GetEntityComponent<StatsComponent>().GetStats<Stats>();

        targetHealth.ChangeCurrentHealth(-Mathf.Max(0, ownerStats.Damage - targetStats.Armor));

        if (!targetHealth.IsDead())
        {
            return;
        }

        _combatComponent.TriggerOnEnemyKilled();
        _combatComponent.SetIdle();
    }

    public override void Attack(Entity owner, Entity target, Transform projectileSpawnPoint)
    {
        return;
    }
}