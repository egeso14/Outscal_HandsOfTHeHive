using UnityEngine;

public class VelocityMatchingBehavior : ISteeringBehavior
{
    public Transform rootTransform;
    public float maxAcceleration;
    public Collider[] targets;
    public Rigidbody rigidBody;

    private SteeringOutput lastOutput;

    public VelocityMatchingBehavior(Transform rootTransform, float maxAcceleration, Rigidbody rigidbody)
    {
        this.rootTransform = rootTransform;
        this.maxAcceleration = maxAcceleration;
        this.rigidBody = rigidbody;
        this.targets = new Collider[0]; // Initialize with an empty array
        this.lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {

        // we want velocities that are different than ours to have a stronger effect
        SteeringOutput output = new SteeringOutput();
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;
        foreach (var target in targets)
        {
             Rigidbody targetBody = target.GetComponentInParent<Rigidbody>();

             var dotProduct = Vector3.Dot(targetBody.linearVelocity, rigidBody.linearVelocity);
             var strength = 1 - dotProduct;
             averageVelocity += targetBody.linearVelocity * strength;
             count++;
            
        }
        if (count > 0)
        {
            averageVelocity /= count;
            Vector3 acceleration = averageVelocity - rigidBody.linearVelocity;
            float strength = Mathf.Min(acceleration.magnitude, maxAcceleration);
            output.linear += strength * acceleration.normalized;
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
        return false; // Velocity matching behavior is ongoing, it doesn't complete like a path following behavior
    }

    public void DebugBehavior()
    {
        // Debugging logic can be added here if needed
        Debug.DrawRay(rootTransform.position, lastOutput.linear, Color.green);
    }
}
