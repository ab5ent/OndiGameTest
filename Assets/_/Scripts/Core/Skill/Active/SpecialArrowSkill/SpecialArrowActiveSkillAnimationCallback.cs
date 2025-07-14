using UnityEngine;

public class SpecialArrowActiveSkillAnimationCallback : SkillAnimationCallback
{
    [SerializeField] private Transform spawnPoint;

    public override void OnSkillCompleteCallback()
    {
        AnimationComponent.Owner.GetEntityComponent<SkillsComponent>().OnSkillComplete();
    }

    public void OnAttackCallback()
    {
        AnimationComponent.Owner.GetEntityComponent<SkillsComponent>()
            .GetActiveSkill<SpecialArrowActiveSkill>().OnAttack(spawnPoint);
    }
}