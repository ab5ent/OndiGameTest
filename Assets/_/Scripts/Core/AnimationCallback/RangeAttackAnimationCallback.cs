using UnityEngine;

public class RangeAttackAnimationCallback : AnimationCallback
{
    [SerializeField] private Transform projectileSpawnPoint;

    private void RangeAttackCallback()
    {
        AnimationComponent.Owner.GetEntityComponent<CombatComponent>()
            .OnAttack("RangeAttack", projectileSpawnPoint);
    }
}