using UnityEngine;

public class HitboxComponent : EntityComponent
{
    [SerializeField] private Rigidbody2D rd2D;

    [SerializeField] private BoxCollider2D hitBox;
}