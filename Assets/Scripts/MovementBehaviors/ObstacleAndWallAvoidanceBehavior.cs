using NUnit.Framework.Interfaces;
using UnityEngine;

public class ObstacleAndWallAvoidanceBehavior : ISteeringBehavior
{
    private float avoidDistance;
    private float rayLength;
    private Transform transform;
    private Rigidbody rigidBody;
    private LayerMask raycastMask;
    private SeekBehavior seek;
    private bool isCompleted;
    private SteeringOutput lastOutput;

    public ObstacleAndWallAvoidanceBehavior(Transform transform, Rigidbody rigidBody, 
                            float maxAcceleration, float avoidDistance, float rayLength,
                            LayerMask raycastMask)
    {
        this.transform = transform;
        this.avoidDistance = avoidDistance;
        this.rayLength = rayLength;
        this.rigidBody = rigidBody;
        this.raycastMask = raycastMask;
        seek = new SeekBehavior(transform, Vector3.zero, maxAcceleration);
        lastOutput = new SteeringOutput();
    }

    public SteeringOutput GetSteering()
    {
        if (rigidBody == null)
        {
            Debug.Log("rigidBody is the issue");
        }
        if (transform == null)
        {
            Debug.Log("transform is the issue");
        }
        if (Physics.Raycast(transform.position, rigidBody.linearVelocity,out RaycastHit hitInfo, rayLength, raycastMask))
        {
            Vector3 target = hitInfo.point + hitInfo.normal * avoidDistance;
            seek.SetTarget(target);
            lastOutput = seek.GetSteering();
            return lastOutput;
        }
             
        return new SteeringOutput();
    }



    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void DebugBehavior()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, lastOutput.linear.normalized + transform.position);
    }

}
