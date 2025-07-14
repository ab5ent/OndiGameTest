using UnityEngine;

public class AnimationComponent : EntityComponent
{
    [SerializeField] private Animator eAnimator;

    public void PlayAnimation(string animationName)
    {
        eAnimator.CrossFade(animationName, 0.1f);
    }

    public void SetBool(string parameterName, bool value)
    {
        eAnimator.SetBool(parameterName, value);
    }

    public void SetFloat(string parameterName, float value)
    {
        eAnimator.SetFloat(parameterName, value);
    }

    public void SetTrigger(string parameterName)
    {
        eAnimator.SetTrigger(parameterName);
    }
}