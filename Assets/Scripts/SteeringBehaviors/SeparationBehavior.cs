using UnityEngine;
using UnityEngine.Rendering;

public class SeparationBehavior : ISteeringBehavior
{
    private float maxAcceleration;
    private float tresholdSqr;
    private float decayCoefficient;
    private Transform rootTransform;
    private Collider[] targets; // targets can be in front or all around, up to implementation
    private SteeringOutput lastOutput;

    public SeparationBehavior(Transform rootTransform, float treshold, float decayCoefficient, float maxAcceleration)
    {
        this.rootTransform = rootTransform;
        this.tresholdSqr = treshold * treshold;
        this.maxAcceleration = maxAcceleration;
        this.decayCoefficient = decayCoefficient;
        this.targets = new Collider[0]; // Initialize with an empty array
        this.lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        SteeringOutput output = new SteeringOutput();
        
        foreach (var target in targets)
        {
            if (target.transform.position == rootTransform.position)
                continue; //its us

            Vector3 dif = target.transform.position - rootTransform.position;
            float distanceSqr = dif.sqrMagnitude;

            if (distanceSqr < tresholdSqr)
            {
                var strength = Mathf.Min(decayCoefficient / distanceSqr, maxAcceleration); // is there a better way to make a similar computation?
                output.linear -= strength * dif.normalized;
            }
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
            Debug.DrawLine(rootTransform.position, lastOutput.linear.normalized + rootTransform.position, Color.red);
        }
    }
}
