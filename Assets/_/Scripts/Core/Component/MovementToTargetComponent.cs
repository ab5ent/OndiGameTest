using System;
using UnityEngine;

public class MovementToTargetComponent : EntityComponent
{
    [SerializeField]
    private Vector3 targetPosition;

    [SerializeField]
    private float moveSpeed = 2f;

    private bool _isMoving = false;

    private bool _isEnable = true;

    public event Action OnMeetTarget;

    private void Update()
    {
        if (!_isMoving || !_isEnable)
        {
            return;
        }

        float step = moveSpeed * Time.deltaTime;

        Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, targetPosition, step);

        if (IsMeetTargetPoint())
        {
            StopMoving();
            OnMeetTarget?.Invoke();
        }
    }

    public void MoveToPosition(Vector3 position)
    {
        if (!_isEnable)
        {
            return;
        }

        targetPosition = position;
        _isMoving = true;
    }

    private bool IsMeetTargetPoint()
    {
        return Owner.transform.position == targetPosition;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void SetMoveSpeed(float movementSpeed)
    {
        moveSpeed = movementSpeed;
    }

    public void SetEnable(bool value)
    {
        _isEnable = value;
    }

    public bool IsEnable()
    {
        return _isEnable;
    }
}