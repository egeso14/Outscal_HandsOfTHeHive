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
        Vector3 bestCurrentRelativePosition = Vector3.zero;
        Vector3 bestRelativePosition = Vector3.zero;
        Vector3 bestRelativeVelocity = Vector3.zero;
        float bestTimeOfClosestApproach = 0;

        Collider[] collidersInRange = Physics.OverlapSphere(rootTransform.position, detectionRadius, detectionMask);
        foreach (Collider collider in collidersInRange)
        {
            Rigidbody otherBody = collider.GetComponentInParent<Rigidbody>();
            Debug.Assert(otherBody != null);
            Vector3 relativePos = collider.transform.position - rootTransform.position;
            Vector3 relativeVelocity = otherBody.linearVelocity - rigidBody.linearVelocity;
            float timeOfClosestApproach = Vector3.Dot(relativePos, relativeVelocity) / relativeVelocity.sqrMagnitude;
            // will there be a collision
            var closestDistance = relativePos.magnitude - timeOfClosestApproach * relativeVelocity.magnitude; // estimate of how far apart they will be
            if (closestDistance > 2 * collisionRadius)
            {
                continue;
            }

            // also need to check against 0 here because that also means they don't collide
            if (timeOfClosestApproach > 0 && bestTimeOfClosestApproach > timeOfClosestApproach)
            {
                // this one is best so far
                // save some data
                // don't do unnecessary computations here because they might end up being useless
                bestTarget = collider;
                bestMinDistance = closestDistance;
                bestCurrentDistance = (bestTarget.transform.position - rootTransform.position).magnitude;
                bestCurrentRelativePosition = relativePos;
                bestRelativePosition = 
                bestRelativeVelocity = Vector3.zero;
                bestTimeOfClosestApproach = 0;
            }
        }

        Vector3 finalRelativePos = Vector3.zero;

        if (bestTarget == null)
        {
            return new SteeringOutput();
        }
        // if we are already colliding or if we will hit exactly
        if (bestMinDistance <= 0 || bestCurrentDistance.magnitude <= 2 * collisionRadius)
        {
            finalRelativePos = bestTarget.transform.position - rootTransform.position;
        }
        else
        {
            finalRelativePos = bestRelativePosition + bestRelativeVelocity * bestTimeOfClosestApproach;
        }

        finalRelativePos.Normalize();
        SteeringOutput output = new SteeringOutput();
        output.linear = -1 * finalRelativePos * maxAcceleration;
        return output;
    }

    public bool IsCompleted()
    {

    }
}
