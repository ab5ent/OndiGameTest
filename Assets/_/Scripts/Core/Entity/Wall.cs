public class Wall : Entity
{
    public override void Initialize()
    {
        int maxHealth = GetEntityComponent<StatsComponent>().GetStats<WallStats>().MaxHealth;
        GetEntityComponent<HealthComponent>().SetMaxHealth(maxHealth);
    }
}