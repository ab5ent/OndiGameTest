using DG.Tweening;
using UnityEngine;

public class ParabolicMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public float duration = 2f;
    public float arcHeight = 5f;
    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Control")]
    public bool playOnStart = true;
    public bool loopMovement = false;
    
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 midPoint;
    private Tween moveTween;
    
    void Start()
    {
        if (playOnStart)
        {
            SetupMovement();
            StartParabolicMovement();
        }
    }
    
    void SetupMovement()
    {
        // Get positions
        startPos = startPoint != null ? startPoint.position : transform.position;
        endPos = endPoint != null ? endPoint.position : transform.position + Vector3.forward * 10f;
        
        // Calculate mid point for the arc
        midPoint = (startPos + endPos) / 2f;
        midPoint.y += arcHeight;
        
        // Set initial position
        transform.position = startPos;
    }
    
    public void StartParabolicMovement()
    {
        // Kill any existing tween
        moveTween?.Kill();
        
        // Create the parabolic path using DOTween's path system
        Vector3[] path = new Vector3[] { startPos, midPoint, endPos };
        
        // Move along the path
        moveTween = transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLoops(loopMovement ? -1 : 1, LoopType.Restart)
            .OnComplete(() => {
                if (!loopMovement)
                    Debug.Log("Parabolic movement completed!");
            });
    }
    
    // Alternative method using manual calculation
    public void StartParabolicMovementManual()
    {
        moveTween?.Kill();
        
        float t = 0f;
        moveTween = DOTween.To(() => t, x => t = x, 1f, duration)
            .OnUpdate(() => {
                Vector3 pos = CalculateParabolicPoint(startPos, endPos, t);
                transform.position = pos;
            })
            .SetEase(Ease.Linear)
            .SetLoops(loopMovement ? -1 : 1, LoopType.Restart);
    }
    
    Vector3 CalculateParabolicPoint(Vector3 start, Vector3 end, float t)
    {
        // Linear interpolation for X and Z
        Vector3 linearPoint = Vector3.Lerp(start, end, t);
        
        // Parabolic curve for Y using the height curve
        float heightOffset = heightCurve.Evaluate(t) * arcHeight;
        float baseHeight = Mathf.Lerp(start.y, end.y, t);
        
        return new Vector3(linearPoint.x, baseHeight + heightOffset, linearPoint.z);
    }
    
    // Method to start movement with custom parameters
    public void StartParabolicMovement(Vector3 customStart, Vector3 customEnd, float customDuration = -1f, float customHeight = -1f)
    {
        startPos = customStart;
        endPos = customEnd;
        
        if (customDuration > 0) duration = customDuration;
        if (customHeight > 0) arcHeight = customHeight;
        
        transform.position = startPos;
        StartParabolicMovement();
    }
    
    // Control methods
    public void PauseMovement()
    {
        moveTween?.Pause();
    }
    
    public void ResumeMovement()
    {
        moveTween?.Play();
    }
    
    public void StopMovement()
    {
        moveTween?.Kill();
    }
    
    // Reset to start position
    public void ResetPosition()
    {
        moveTween?.Kill();
        transform.position = startPos;
    }
    
    void OnDestroy()
    {
        moveTween?.Kill();
    }
    
    // Gizmos for visualization in Scene view
    void OnDrawGizmos()
    {
        if (startPoint == null || endPoint == null) return;
        
        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;
        Vector3 mid = (start + end) / 2f;
        mid.y += arcHeight;
        
        // Draw the parabolic arc
        Gizmos.color = Color.yellow;
        Vector3 prevPoint = start;
        
        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;
            Vector3 currentPoint = CalculateParabolicPoint(start, end, t);
            Gizmos.DrawLine(prevPoint, currentPoint);
            prevPoint = currentPoint;
        }
        
        // Draw start and end points
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(end, 0.5f);
        
        // Draw mid point
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mid, 0.3f);
    }
}