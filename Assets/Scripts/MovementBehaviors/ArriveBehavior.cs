using UnityEngine;
using UnityEngine.Rendering;

public class ArriveBehavior
{
    private Rigidbody rigidBody;
    private Transform transform;
    private Vector3 target;
    private float maxAcceleration;
    private float maxSpeed;
    private float stopRadius;
    private float slowRadius;
    private float timeToTarget;

    public ArriveBehavior(Rigidbody rigidBody, Transform transform, Vector3 target,
                          float maxAcceleration, float maxSpeed, float stopRadius,
                          float slowRadius, float timeToTarget)
    {
        this.rigidBody = rigidBody;
        this.transform = transform;
        this.target = target;
        this.maxAcceleration = maxAcceleration;
        this.maxSpeed = maxSpeed;
        this.stopRadius = stopRadius;
        this.slowRadius = slowRadius;
        this.timeToTarget = timeToTarget;
    }

    public void Update(SteeringOutput outputToFill)
    {
        var direction = target - transform.position;

    }


}
