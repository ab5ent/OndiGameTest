using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class UserControllerComponent : EntityComponent
{
    [field: SerializeField]
    public bool IsSelected { get; protected set; }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }

    private Vector3 _lastMousePosition;

    public void Update()
    {
        if (!IsSelected)
        {
            return;
        }

        HandleMoveToPosition();
        HandleUsingSkill();
    }

    private void HandleUsingSkill()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Owner.GetEntityComponent<SkillsComponent>().UseActiveSkill();
        }
    }

    private void HandleMoveToPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!Owner.GetEntityComponent<MovementToTargetComponent>().IsEnable())
            {
                return;
            }

            if (Owner.GetEntityComponent<HealthComponent>().IsDead())
            {
                return;
            }

            _lastMousePosition = Input.mousePosition;

            Vector3 targetPosition = Camera.main!.ScreenToWorldPoint(_lastMousePosition);
            targetPosition.z = 0;

            Owner.GetEntityComponent<MovementToTargetComponent>().MoveToPosition(targetPosition);
            Owner.GetEntityComponent<RendererComponent>().SetRotate(Owner.transform.position - targetPosition);

            Owner.GetEntityComponent<CombatComponent>().DisableAttack();
            Owner.GetEntityComponent<AnimationComponent>().SetBool("isAttacking", false);
            Owner.GetEntityComponent<AnimationComponent>().SetFloat("movement", 1);
        }
    }
}