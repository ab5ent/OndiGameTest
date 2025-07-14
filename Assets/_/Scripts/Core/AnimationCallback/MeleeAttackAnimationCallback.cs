using UnityEngine;

public class MeleeAttackAnimationCallback : AnimationCallback
{
    private void MeleeAttackCallback()
    {
        AnimationComponent.Owner.GetEntityComponent<CombatComponent>().OnAttack("MeleeAttack");
    }
}