using UnityEngine;

public class Stats : MonoBehaviour
{
    [field: SerializeField]
    public int MaxHealth { get; private set; }

    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public int Armor { get; private set; }

    [field: SerializeField]
    public float MovementSpeed { get; private set; }
    
    [field: SerializeField]
    public float HealthRegenPerSecond { get; private set; }
}