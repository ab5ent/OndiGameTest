using UnityEngine;
using UnityEngine.Serialization;

public class TestHealthStats : MonoBehaviour
{
    [FormerlySerializedAs("_healthStats"),SerializeField] private HealthComponent healthComponent;

    [SerializeField]
    private int takeDamageValue;

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        healthComponent.ChangeCurrentHealth(takeDamageValue);
    }

    private void Update()
    {
        healthComponent.Regenerate();
    }
}