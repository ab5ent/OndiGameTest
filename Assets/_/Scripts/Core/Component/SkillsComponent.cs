using System.Collections.Generic;
using UnityEngine;

public class SkillsComponent : EntityComponent
{
    [SerializeField] private List<Skill> passiveSkills;

    [SerializeField] private Skill activeSkill;

    public void SetPassiveSkills(Skill[] newPassiveSkills)
    {
        if (passiveSkills.Count > 0)
        {
            foreach (Skill passiveSkill in passiveSkills)
            {
                if (passiveSkill != null)
                {
                    passiveSkill.OnUnequip();
                }
            }

            passiveSkills.Clear();
        }

        foreach (Skill newPassiveSkill in newPassiveSkills)
        {
            Skill passiveSkill = Instantiate(newPassiveSkill, transform);
            passiveSkill.OnEquip(Owner);
            passiveSkills.Add(passiveSkill);
        }
    }

    public void SetActiveSkill(Skill newActiveSkill)
    {
        if (activeSkill != null)
        {
            activeSkill.OnUnequip();
            activeSkill = null;
        }

        activeSkill = Instantiate(newActiveSkill, transform);
        activeSkill.OnEquip(Owner);
    }

    public void UseActiveSkill()
    {
        activeSkill.UseSkill();
    }

    public void OnSkillComplete()
    {
        activeSkill.OnSkillComplete();
    }

    public T GetActiveSkill<T>() where T : Skill
    {
        return (T)activeSkill;
    }
}