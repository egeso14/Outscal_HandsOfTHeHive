using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PathFollower
{

    private Transform rootTransform;
    private List<Vector3> path;
    private int currentIndex;
    private Vector2 lastPos;

    /// <summary>
    /// This class works under the assumption that 
    /// </summary>
    public PathFollower(List<Vector3> path, Transform rootTransform)
    {
        this.path = path;
        this.rootTransform = rootTransform;
        lastPos = rootTransform.position;
        currentIndex = 0;
    }

    public Vector3 GetNextTarget()
    {
        CheckUpdateTarget();
        if (currentIndex == path.Count) return Vector3.zero;
        return path[currentIndex];
    }
    public bool DestinationReached()
    {
        return path.Count == currentIndex;
    }

    private void CheckUpdateTarget()
    {
        if (lastPos == (Vector2)rootTransform.position)
        {
            return;
        }

        Vector2 beforeOffset = ((Vector2)path[currentIndex] - lastPos).normalized;
        Vector2 afterOffset = ((Vector2)path[currentIndex] - (Vector2)rootTransform.position).normalized;
        
        lastPos = rootTransform.position;
        
        if (Vector2.Dot(beforeOffset, afterOffset) < 0)
        {
            currentIndex++;
        }
    }

    
}
