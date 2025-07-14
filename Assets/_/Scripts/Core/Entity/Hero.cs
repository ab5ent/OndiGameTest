using UnityEngine;

public class Hero : Character
{
    [SerializeField]
    protected EHeroName eHeroName;

    protected override void Awake()
    {
        base.Awake();

        GetEntityComponent<LevelComponent>().OnLevelUp += LevelUp;
        GetEntityComponent<HealthComponent>().OnDead += OnDeadCallback;
        GetEntityComponent<RespawnComponent>().OnRespawn += OnRespawn;
        GetEntityComponent<MovementToTargetComponent>().OnMeetTarget += OnMovedToPosition;
    }

    protected void OnDestroy()
    {
        GetEntityComponent<LevelComponent>().OnLevelUp -= LevelUp;
        GetEntityComponent<HealthComponent>().OnDead -= OnDeadCallback;
        GetEntityComponent<RespawnComponent>().OnRespawn -= OnRespawn;
        GetEntityComponent<MovementToTargetComponent>().OnMeetTarget -= OnMovedToPosition;
    }

    private void LevelUp(int currentLevel)
    {
        HeroLevelData heroLevelData = DataController.Instance.GetHeroLevelData(eHeroName, currentLevel);

        HeroStats heroStats = heroLevelData.GetHeroStats();

        GetEntityComponent<StatsComponent>().SetStats(heroStats);
        GetEntityComponent<HealthComponent>().SetMaxHealth(heroStats.MaxHealth);
        GetEntityComponent<HealthComponent>().SetHealthRegenPerSecond(heroStats.HealthRegenPerSecond);

        GetEntityComponent<MovementToTargetComponent>().SetMoveSpeed(heroStats.MovementSpeed);
        GetEntityComponent<RespawnComponent>().SetRespawnTime(heroStats.RespawnTime);

        GetEntityComponent<SkillsComponent>().SetPassiveSkills(heroLevelData.GetPassiveSkills());
        GetEntityComponent<SkillsComponent>().SetActiveSkill(heroLevelData.GetActiveSkill());
    }

    private void OnDeadCallback()
    {
        GetEntityComponent<CombatComponent>().DisableAttack();

        GetEntityComponent<AnimationComponent>().SetBool("isAttacking", false);
        GetEntityComponent<AnimationComponent>().PlayAnimation("Die");

        GetEntityComponent<UserControllerComponent>().Deselect();
        GetEntityComponent<RespawnComponent>().StartRespawnCooldown();
    }

    private void OnRespawn()
    {
        GetEntityComponent<CombatComponent>().EnableAttack();

        GetEntityComponent<AnimationComponent>().SetTrigger("respawn");
        GetEntityComponent<AnimationComponent>().SetFloat("movement", 0);
        GetEntityComponent<AnimationComponent>().SetBool("isAttacking", false);

        GetEntityComponent<HealthComponent>().RefreshHealth();
    }

    private void OnMovedToPosition()
    {
        GetEntityComponent<AnimationComponent>().SetFloat("movement", 0);
        GetEntityComponent<CombatComponent>().EnableAttack();
    }

    public override void Initialize()
    {
        LevelUp(0);
        GetEntityComponent<UserControllerComponent>().Select();
    }
}