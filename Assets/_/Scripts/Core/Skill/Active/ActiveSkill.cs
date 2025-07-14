using UnityEngine;

public abstract class ActiveSkill : Skill
{
    [SerializeField] private string skillName;
    
    public abstract bool CanUseSkill(); 
}