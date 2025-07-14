using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IEntityComponent
{
    public Entity Owner { get; private set; }

    public virtual void Initialize(Entity owner)
    {
        Owner = owner;
    }
}