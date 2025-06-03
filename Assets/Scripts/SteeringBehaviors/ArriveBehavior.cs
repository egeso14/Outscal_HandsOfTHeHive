using System.Drawing;
using UnityEngine;


public class ArriveBehavior : ISteeringBehavior
{
    private Transform rootTransform;
    private Rigidbody rigidbody;
    private Vector3 target;
    private float maxAcceleration;
    private float maxSpeed;
    private float stopRadius;
    private float slowRadius;
    private float timeToTarget;
    private SteeringOutput lastOutput;

    public ArriveBehavior(Rigidbody rigidbody, Transform transform, Vector3 target,
                          float maxAcceleration, float maxSpeed, float stopRadius,
                          float slowRadius, float timeToTarget)
    { 
        this.rigidbody = rigidbody;
        this.rootTransform = transform;
        this.target = target;
        this.maxAcceleration = maxAcceleration;
        this.maxSpeed = maxSpeed;
        this.stopRadius = stopRadius;
        this.slowRadius = slowRadius;
        this.timeToTarget = timeToTarget;
        this.lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        Vector3 dif = target - rootTransform.position;
        float magnitude = dif.magnitude;

        // check if we are inside the stop radius
        if (magnitude < stopRadius)
        {
            // if we are, we should return no steering since we don't want to oscillate
            lastOutput = new SteeringOutput();

            return lastOutput;
        }

        float targetSpeed = maxSpeed;

        if (magnitude < slowRadius)
        {
            targetSpeed = maxSpeed * magnitude / slowRadius;
        }

        Vector3 targetVelocity = dif.normalized * targetSpeed;
        Vector3 accelerationToGetThere = (targetVelocity - rigidbody.linearVelocity) / timeToTarget;
        lastOutput = new SteeringOutput();
        lastOutput.linear = accelerationToGetThere;

        if (accelerationToGetThere.magnitude >  maxAcceleration)
        {
            lastOutput.linear = lastOutput.linear.normalized * maxAcceleration;
        }
        return lastOutput;
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    public bool IsCompleted()
    {
        return (rootTransform.position - target).magnitude < stopRadius;
    }

    public virtual void DebugBehavior()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawLine(rootTransform.position, rootTransform.position + lastOutput.linear.normalized);
    }


}
