using UnityEngine;
using UnityEngine.Rendering;

public class WanderBehavior : ISteeringBehavior
{
    private bool isCompleted;
    private Vector3 wanderOrientation; // the orientation of the wander target, need to keep track of this to keep things stable
    private float wanderRadius;
    private float wanderOffset;
    private float wanderRate;
    private float maxAcceleration;
    private Transform rootTransform;
    private SeekBehavior seek;

    public WanderBehavior(float wanderRadius, float wanderOffset, float wanderRate, float maxAcceleration, Transform rootTransform)
    {
        this.wanderRadius = wanderRadius;
        this.wanderOffset = wanderOffset;
        this.wanderRate = wanderRate;
        this.maxAcceleration = maxAcceleration;
        this.rootTransform = rootTransform;
        seek = new SeekBehavior(rootTransform, Vector3.zero, maxAcceleration);
    }

    public SteeringOutput GetSteering()
    {
        wanderOrientation += Random.onUnitSphere * Random.Range(0f, 1f) * wanderRate;
        var targetOrientation = rootTransform.rotation * wanderOrientation;
        var target = rootTransform.position + rootTransform.forward * wanderOffset;
        target += wanderRadius * wanderOrientation;
        seek.SetTarget(target);

        SteeringOutput output = seek.GetSteering();
        return output;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void DebugBehavior()
    {

    }
}
