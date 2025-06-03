using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LinePath : IPath
{
    public List<Vector3> points;
    private int pathLookAhead;
    public LinePath(List<Vector3> points, int pathLookAhead)
    {
        this.points = points;
        this.pathLookAhead = pathLookAhead;
    }
    public float GetParameter(Vector3 position, float lastParam)
    {
        float bestSqrDist = float.MaxValue;
        int bestIndex = -1;
        for (int i = (int)lastParam; i < lastParam + pathLookAhead; i++)
        {
            if (i > points.Count - 1)
            {
                break; // Avoid going out of bounds
            }
            float sqrDist = (position - points[i]).sqrMagnitude;
            // calculate the closest point on the list to position ahead of the lastParam
            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                bestIndex = i;
            }
        }
        if (bestIndex == -1)
        {
            return lastParam; // No valid point found, return the last parameter
        }
        return bestIndex;
    }
    public Vector3 GetPosition(float param)
    {
        if (param > points.Count - 1)
        {
            return points[points.Count - 1];
        }

        return points[(int)param];
        /*Vector3 prev = points[(int)param];
        Vector3 next = points[(int)param + 1];
        return points[(int)param] + (next - prev).normalized * (param - (int) param);*/
    }

    public bool IsEndOfPath(float param)
    {
        return param >= points.Count - 1;
    }

    public void DebugPath()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(points[i], points[i + 1], Color.cyan);
        }
    }

    private static Vector3 ClosestPointOnSegment(Vector3 A, Vector3 B, Vector3 P)
    {
        Vector3 AB = B - A;
        float abLenSq = Vector3.Dot(AB, AB);
        if (abLenSq < Mathf.Epsilon) return A;  // A and B are effectively the same point

        // t = projection of AP onto AB, normalized [0,1]
        float t = Vector3.Dot(P - A, AB) / abLenSq;
        t = Mathf.Clamp01(t);

        return A + AB * t;
    }

}
