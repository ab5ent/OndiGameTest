using UnityEngine;

public class SpecialArrowActiveSkill : ActiveSkill
{
    private Entity _target;

    [SerializeField] private SimpleProjectile projectilePrefab;

    public override void OnEquip(Entity entity)
    {
        SetEntity(entity);
        _canUseSkill = true;
    }

    public override void UseSkill()
    {
        if (CanUseSkill())
        {
            TriggerSkill();
        }
        else
        {
            Debug.Log("Can't use skill!");
        }
    }

    private void TriggerSkill()
    {
        BeginCooldown();

        CombatComponent combatComponent = Owner.GetEntityComponent<CombatComponent>();

        _target = combatComponent.GetCurrentTarget();

        if (_target == null)
        {
            _target = combatComponent.GetNearestTarget();
        }

        combatComponent.DisableAttack();

        Owner.GetEntityComponent<MovementToTargetComponent>().StopMoving();
        Owner.GetEntityComponent<MovementToTargetComponent>().SetEnable(false);
        Owner.GetEntityComponent<AnimationComponent>().PlayAnimation("Skill");
        Owner.GetEntityComponent<RendererComponent>().SetRotate(Owner.transform.position - _target.transform.position);
    }

    public override void OnUnequip()
    {
        return;
    }

    public override void OnSkillComplete()
    {
        Owner.GetEntityComponent<AnimationComponent>().SetTrigger("isSkillCompleted");
        Owner.GetEntityComponent<CombatComponent>().EnableAttack();
        Owner.GetEntityComponent<MovementToTargetComponent>().SetEnable(true);
    }

    private void Update()
    {
        UpdateCooldown();
    }

    public void OnAttack(Transform spawnPoint)
    {
        SimpleProjectile projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        projectile.gameObject.SetActive(true);
        projectile.transform.SetParent(null);
        projectile.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

        projectile.Launch(spawnPoint, _target.transform, () =>
        {
            HealthComponent targetHealth = _target.GetEntityComponent<HealthComponent>();
            Stats targetStats = _target.GetEntityComponent<StatsComponent>().GetStats<Stats>();

            Stats ownerStats = Owner.GetEntityComponent<StatsComponent>().GetStats<Stats>();
            targetHealth.ChangeCurrentHealth(-Mathf.Max(0, ownerStats.Damage * 2 - targetStats.Armor));

            if (targetHealth.IsDead())
            {
                Owner.GetEntityComponent<CombatComponent>().TriggerOnKillAEnemy(_target);
            }

            Destroy(projectile.gameObject);
        });
    }

    public override bool CanUseSkill()
    {
        bool hasTargets = Owner.GetEntityComponent<CombatComponent>().HasTargets();
        return hasTargets && _canUseSkill;
    }
}