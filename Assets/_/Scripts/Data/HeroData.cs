using UnityEngine;

public class HeroData : MonoBehaviour
{
    [SerializeField] private EHeroName eHeroName;

    [SerializeField] private HeroLevelData[] heroLevelData;

    public HeroStats GetHeroStats(int currentLevel)
    {
        return GetHeroLevelData(currentLevel).GetHeroStats();
    }

    public HeroLevelData GetHeroLevelData(int currentLevel)
    {
        return heroLevelData[Mathf.Clamp(currentLevel, 0, heroLevelData.Length - 1)];
    }
}