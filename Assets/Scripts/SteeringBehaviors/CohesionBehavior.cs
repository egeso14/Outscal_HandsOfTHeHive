using System.Runtime.CompilerServices;
using UnityEngine;

public class CohesionBehavior : ISteeringBehavior
{
    private float squaredTreshold;
    private float decayCoefficient;
    private float maxAcceleration;
    private Transform rootTransform;
    private Collider[] targets; // targets can be in front or all around, up to implementation
    private SteeringOutput lastOutput;

    public CohesionBehavior(Transform rootTransform, float maxAcceleration, float treshold, float decayCoefficient)
    {
        this.rootTransform = rootTransform;
        this.maxAcceleration = maxAcceleration;
        this.squaredTreshold = treshold * treshold;
        this.decayCoefficient = decayCoefficient;
        this.targets = new Collider[0]; // Initialize with an empty array
        this.lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();
        Vector3 centerOfMass = Vector3.zero;
        int count = 0;
        foreach (var target in targets)
        {
            Vector3 dif = target.transform.position - rootTransform.position;
            float distanceSqr = dif.sqrMagnitude;
            if (distanceSqr > squaredTreshold)
            {
                centerOfMass += target.transform.position;
                count++;
            }
        }
        if (count > 0)
        {
            centerOfMass /= count;
            Vector3 directionToCenter = centerOfMass - rootTransform.position;
            float strength = Mathf.Min(directionToCenter.sqrMagnitude / decayCoefficient, maxAcceleration);
            output.linear += strength * directionToCenter.normalized;
        }
        lastOutput = output;
        return output;
    }

    public void UpdateTargets(Collider[] newTargets)
    {
        targets = newTargets;
    }

    public bool IsCompleted()
    {
        return false; // Cohesion behavior is ongoing, it doesn't complete like a path following behavior
    }

    public void DebugBehavior()
    {
        if (lastOutput != null)
        {
            Debug.DrawLine(rootTransform.position, lastOutput.linear.normalized + rootTransform.position, Color.magenta);
        }
    }
}
