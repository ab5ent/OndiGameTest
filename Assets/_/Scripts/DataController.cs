using UnityEngine;

public class DataController : MonoBehaviour
{
    [SerializeField] private HeroData[] heroDatas;

    public static DataController Instance { get; protected set; }

    private void Awake()
    {
        Instance = this;
    }

    public HeroStats GetHeroStats(EHeroName eHeroName, int currentLevel)
    {
        return heroDatas[(int)eHeroName].GetHeroStats(currentLevel);
    }

    public HeroLevelData GetHeroLevelData(EHeroName eHeroName, int currentLevel)
    {
        return heroDatas[(int)eHeroName].GetHeroLevelData(currentLevel);
    }
}