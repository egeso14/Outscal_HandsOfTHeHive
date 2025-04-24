using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class StillStrategy : MovementStrategy
{
    // references
    private BeeBrain myBrain;
    private SwarmParameters swarmParameters;
    // internals
    private float waitAmount;
    private float waitTime;

    private float moveAmount;
    private float moveTime;

    private bool waiting;
    public StillStrategy(BeeBrain brain)
    {
        myBrain = brain;
        swarmParameters = SwarmParameters.Instance;

        waitTime = 0;
        waitAmount = 0.5f;

        
    }

    public Vector3 CalculateVelocityVector()
    {
        // fix this by adding determineMoving function
        if (waiting) 
        {
            waitTime -= Time.fixedDeltaTime;
            if (waitTime < 0)
            {
                waitTime = waitAmount;
                waiting = false;
                return CalculateMovingVelocity();
            }
            return CalculateWaitingVelocity();
        }
        else
        {
            moveTime -= Time.fixedDeltaTime;
            if (moveTime < 0)
            {
                moveTime = moveAmount;
                waiting = true;
                return CalculateWaitingVelocity();
            }
            return CalculateMovingVelocity();
        }
    }

    private Vector3 CalculateWaitingVelocity()
    {
        return Vector3.zero;
    }

    private Vector3 CalculateMovingVelocity()
    {
        var detectedColliders = Physics.OverlapSphere(myBrain.transform.position,
            swarmParameters.detectionRadius);
        

        Vector3 cohesionSum = Vector3.zero;
        Vector3 avoidanceSum = Vector3.zero;
        

        foreach (var collider in detectedColliders)
        {
            Vector3 colliderPosition = collider.transform.position;

            var distance = colliderPosition - myBrain.transform.position;

            cohesionSum += GetCohesionWeight(distance.sqrMagnitude) * colliderPosition;
            avoidanceSum += GetAvoidanceWeight(distance.sqrMagnitude) * colliderPosition;
        }

        

    }

    private float GetCohesionWeight(float distance)
    {
        return Mathf.InverseLerp(swarmParameters.cohesionMinRange,
            swarmParameters.cohesionMaxRange, distance);
    }

    private float GetAvoidanceWeight(float distance)
    {
        return Mathf.InverseLerp(swarmParameters.avoidanceMinRange,
            swarmParameters.avoidanceMaxRange, distance);
    }

}
