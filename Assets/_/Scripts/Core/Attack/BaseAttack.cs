using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{
    [field: SerializeField]
    public string AnimationName { get; protected set; }

    protected CombatComponent _combatComponent;

    public void Initialize(CombatComponent combatComponent)
    {
        _combatComponent = combatComponent;
    }

    public abstract void Attack(Entity owner, Entity target);

    public abstract void Attack(Entity owner, Entity target, Transform projectileSpawnPoint);
}