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
    }

    public SteeringOutput GetSteering()
    {
        if (Physics.Raycast(transform.position, rigidBody.linearVelocity,out RaycastHit hitInfo, rayLength, raycastMask))
        {
            Vector3 target = hitInfo.point + hitInfo.normal * avoidDistance;
            seek.SetTarget(target);
            return seek.GetSteering();
        }
        return new SteeringOutput();
    }

    public void DebugBehavior()
    {

    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}
