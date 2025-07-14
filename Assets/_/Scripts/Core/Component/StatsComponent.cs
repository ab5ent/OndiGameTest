using UnityEngine;
using UnityEngine.Serialization;

public class StatsComponent : EntityComponent
{
    [SerializeField] private Stats originStats;

    [SerializeField] private Stats currentStats;

    public T GetStats<T>() where T : Stats
    {
        if (currentStats is T resultStats)
        {
            return resultStats;
        }

        return null;
    }

    public void SetStats<T>(T newStats) where T : Stats
    {
        if (originStats is not T)
        {
            return;
        }

        if (originStats)
        {
            Destroy(originStats.gameObject);
        }

        if (currentStats)
        {
            Destroy(currentStats.gameObject);
        }

        originStats = Instantiate(newStats, transform);
        currentStats = Instantiate(newStats, transform);

        currentStats.name += " (Current)";
    }
}