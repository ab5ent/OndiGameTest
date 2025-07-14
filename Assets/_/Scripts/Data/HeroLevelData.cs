using UnityEngine;
using UnityEngine.Serialization;

public class HeroLevelData : MonoBehaviour
{
    [FormerlySerializedAs("heroLevelStats"), SerializeField]
    private HeroStats stats;

    [SerializeField] private Skill[] passiveSkills;

    [SerializeField] private Skill activeSkill;

    public HeroStats GetHeroStats()
    {
        return stats;
    }

    public Skill[] GetPassiveSkills()
    {
        return passiveSkills;
    }

    public Skill GetActiveSkill()
    {
        return activeSkill;
    }
}