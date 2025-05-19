using UnityEngine;
using UnityEngine.Rendering;

public class CollisionAvoidanceBehavior : ISteeringBehavior
{
    private Transform rootTransform;
    private Rigidbody rigidBody;
    private float maxAcceleration;
    private float detectionRadius; // keep this small because this is an expensive behavior
    private LayerMask detectionMask;
    private float collisionRadius;
    public void DebugBehavior()
    {

    }

    public SteeringOutput GetSteering()
    {
        Collider bestTarget = null;
        float bestMinDistance = 0;
        float bestCurrentDistance = 0;
        float shortestTimeToCollision = 0;
        Collider[] collidersInRange = Physics.OverlapSphere(rootTransform.position, detectionRadius, detectionMask);
        foreach (Collider collider in collidersInRange)
        {
            Rigidbody otherBody = collider.GetComponentInParent<Rigidbody>();
            Debug.Assert(otherBody != null);
            Vector3 relativePos = rootTransform.position - collider.transform.position;
            Vector3 relativeVelocity = otherBody.linearVelocity - rigidBody.linearVelocity;
            float timeOfClosestApproach = Vector3.Dot(relativePos, relativeVelocity) / relativeVelocity.sqrMagnitude;
            // will there be a collision
            var closestDistance = relativePos.magnitude - timeOfClosestApproach * relativeVelocity.magnitude; // estimate of how far apart they will be
            if (closestDistance > 2 * collisionRadius)
            {
                continue;
            }

            // also need to check against 0 here because that also means they don't collide
            if (timeOfClosestApproach > 0 && shortestTimeToCollision > timeOfClosestApproach)
            {
                // this one is best so far
                // save some data
            }
        }

        var relativePos = 0;

        if (bestTarget == null)
        {
            return new SteeringOutput();
        }

        if (bestMinDistance <= 0 || bestCurrentDistance <= 2 * collisionRadius)
        {
            relativePos = bestTarget.position - 
        }
    }

    public bool IsCompleted()
    {

    }
}
