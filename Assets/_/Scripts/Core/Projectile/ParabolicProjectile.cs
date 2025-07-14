using DG.Tweening;
using System;
using UnityEngine;

public class ParabolicProjectile : SimpleProjectile
{
    public override void Launch(Transform startPoint, Transform endPoint, Action OnHit)
    {
        Vector3[] path = new Vector3[3];

        path[0] = startPoint.position;
        path[1] = CalculateParabolicPoint(startPoint.position, endPoint.position);
        path[2] = endPoint.position;

        Vector3 previousPosition = startPoint.position;

        transform.DOPath(path, time, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                Vector3 currentPosition = transform.position;
                Vector3 direction = (currentPosition - previousPosition).normalized;

                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                previousPosition = currentPosition;
            })
            .OnComplete(() =>
            {
                OnHit?.Invoke();
            });
    }

    private Vector3 CalculateParabolicPoint(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 result = (startPosition + endPosition) / 2f;
        result.y += height;

        if (result.y < endPosition.y)
        {
            result.y = endPosition.y + height;
        }

        return result;
    }
}