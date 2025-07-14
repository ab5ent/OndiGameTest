using UnityEngine;

public class RestoreHealthPerKillPassiveSkill : Skill
{
    [SerializeField] private int healthPerKill;

    public override void OnEquip(Entity entity)
    {
        SetEntity(entity);

        CombatComponent combatComponent = Owner.GetEntityComponent<CombatComponent>();

        if (combatComponent)
        {
            combatComponent.OnEnemyKilled += RestoreHealth;
        }
    }

    public override void UseSkill()
    {
    }

    public override void OnUnequip()
    {
        CombatComponent combatComponent = Owner.GetEntityComponent<CombatComponent>();

        if (combatComponent)
        {
            combatComponent.OnEnemyKilled -= RestoreHealth;
        }

        SetEntity(null);
        Destroy(gameObject);
    }

    private void RestoreHealth()
    {
        Owner.GetEntityComponent<HealthComponent>().ChangeCurrentHealth(healthPerKill);
    }

    public override void OnSkillComplete()
    {
        Owner.GetEntityComponent<HealthComponent>().ChangeCurrentHealth(healthPerKill);
    }
}