using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LinePath : IPath
{
    public List<Vector3> points;
    private int pathLookAhead;
    public LinePath(List<Vector3> points)
    {
        this.points = points;
    }
    public float GetParameter(Vector3 position, float lastParam)
    {
        for (int i = 0; i < lastParam + pathLookAhead)
    }
    public Vector3 GetPosition(float param)
    {
        if (param > points.Count - 1)
        {
            return points[points.Count - 1];
        }

        Vector3 prev = points[(int)param];
        Vector3 next = points[(int)param + 1];
        return points[(int)param] + (next - prev).normalized * (param - (int) param);
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
