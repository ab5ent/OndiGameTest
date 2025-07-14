using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementByPathComponent : EntityComponent
{
    [SerializeField]
    private Transform[] path;

    [SerializeField]
    private float moveSpeed = 2f;

    private bool _isMoving, _isEnable = false;

    private int _currentIndex = 0;

    public void SetPath(Transform[] newPath)
    {
        path = newPath;
    }

    public void StartMove()
    {
        _currentIndex = 0;
        _isMoving = true;
    }

    private void UpdatePosition()
    {
        if (_currentIndex == path.Length - 1)
        {
            
        }
        
        _currentIndex = Mathf.Clamp(_currentIndex + 1, 0, path.Length - 1);
        Owner.GetEntityComponent<RendererComponent>().SetRotate(transform.position - path[_currentIndex].position);
    }

    public void RemoveOnMeetTarget()
    {
        path = null;
        _currentIndex = -1;
        _isMoving = false;
        _isEnable = false;
    }

    private void Update()
    {
        if (!_isMoving && !_isEnable || path == null || path.Length == 0 || _currentIndex >= path.Length)
        {
            return;
        }

        float step = moveSpeed * Time.deltaTime;

        Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, path[_currentIndex].position, step);

        if (Owner.transform.position == path[_currentIndex].position)
        {
            UpdatePosition();
        }
    }

    public void SetEnable(bool value)
    {
        _isEnable = value;
    }

    public bool IsEnable()
    {
        return _isEnable;
    }

    public void SetMoveSpeed(float movementSpeed)
    {
        moveSpeed = movementSpeed;
    }

    public void SetMoving(bool value)
    {
        _isMoving = value;
    }

    public void MoveToClosestPathPoint()
    {
        if (path == null || path.Length == 0)
            return;

        float minDistance = float.MaxValue;
        int closestIndex = 0;
        Vector3 currentPosition = Owner.transform.position;

        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] == null)
                continue;

            float distance = Vector3.Distance(currentPosition, path[i].position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        _currentIndex = closestIndex;
        _isMoving = true;

        Owner.GetEntityComponent<RendererComponent>().SetRotate(transform.position - path[_currentIndex].position);
    }
}