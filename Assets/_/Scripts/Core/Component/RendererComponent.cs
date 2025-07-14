using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererComponent : EntityComponent
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void SetRotation(float angle)
    {
        spriteRenderer.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void SetRotate(Vector3 direction)
    {
        SetRotation(direction.x > 0 ? 0 : 180);
    }
}