using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering;

public class WanderBehavior : ISteeringBehavior
{
    private bool isCompleted;
    private Vector3 wanderTarget; // the orientation of the wander target, need to keep track of this to keep things stable
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
        wanderTarget = Vector3.zero;
    }

    public SteeringOutput GetSteering()
    {
        /*Vector3 randomAxis = Random.onUnitSphere;
        float randomAngle = wanderRate * Random.Range(0f, 1f);
        Quaternion randomRotation = Quaternion.AngleAxis(randomAngle, randomAxis);
        wanderOrientation = randomRotation * wanderOrientation;
        var wanderDirection = wanderOrientation * Vector3.forward; // to convert it to a direction vector*/
        float wanderTargetX = wanderTarget.x + Random.Range(0f, 1) * wanderRate;
        float wanderTargetY = wanderTarget.y + Random.Range(0f, 1) * wanderRate;
        float wanderTargetZ = wanderTarget.z + Random.Range(0f, 1) * wanderRate;

        wanderTarget = new Vector3(wanderTargetX, wanderTargetY, wanderTargetZ);
        wanderTarget.Normalize();


        seek.SetTarget(rootTransform.position + rootTransform.forward * wanderOffset + wanderTarget * wanderRadius);
        return seek.GetSteering();
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void DebugBehavior()
    {

    }
}
