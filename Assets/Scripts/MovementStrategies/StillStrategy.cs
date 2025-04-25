using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class StillStrategy : MovementStrategy
{
    // references
    private BeeBrain myBrain;
    private SwarmParameters swarmParameters;
    private Collider myCollider;
    // internals
    private float waitAmount;
    private float waitTime;

    private float moveAmount;
    private float moveTime;

    private float movesInOneDirectionFor;
    private LayerMask detectionMask;
    // owned helper objects
    private TimedRandomVectorGenerator vectorGenerator;


    private bool waiting;
    public StillStrategy(BeeBrain brain)
    {
        myBrain = brain;
        swarmParameters = SwarmParameters.Instance;
        myCollider = myBrain.GetComponent<Collider>();
        // get these values from swarmParameters
        waitTime = 0;
        waitAmount = 0.5f;
        movesInOneDirectionFor = 1;

        moveAmount = 1;
        moveTime = 0;

        vectorGenerator = new TimedRandomVectorGenerator(movesInOneDirectionFor);

        detectionMask = LayerMask.GetMask("Bee", "FlyZoneBoundaries");
    }

    public Vector3 CalculateVelocityVector()
    {
        // fix this by adding determineMoving function
        
        var isWaiting = DetermineIsWaiting();
        return isWaiting ? CalculateWaitingVelocity()
                           : CalculateMovingVelocity();
    }

    private bool DetermineIsWaiting()
    {
        if (waiting)
        {
            waitTime -= Time.fixedDeltaTime;
            if (waitTime < 0)
            {
                waitTime = waitAmount;
                waiting = false;
               
            }
        }
        else
        {
            moveTime -= Time.fixedDeltaTime;
            if (moveTime < 0)
            {
                moveTime = moveAmount;
                waiting = true;
            }
        }
        return waiting;
    }

    private Vector3 CalculateWaitingVelocity()
    {
        return Vector3.zero;
    }

    private Vector3 CalculateMovingVelocity()
    {
        
        var detectedColliders = Physics.OverlapSphere(myBrain.transform.position,
                                       swarmParameters.detectionRadius,
                                        layerMask: detectionMask);
        

        Vector3 cohesionSum = Vector3.zero;
        Vector3 avoidanceSum = Vector3.zero;
        

        foreach (var collider in detectedColliders)
        {
            if (collider == myCollider) continue;
            Vector3 colliderPosition = collider.ClosestPoint(myBrain.transform.position);
            var distance = colliderPosition - myBrain.transform.position;
            avoidanceSum -= GetAvoidanceWeight(distance.sqrMagnitude) * colliderPosition;
            if (collider.gameObject.CompareTag("Bee"))
            {
                cohesionSum += GetCohesionWeight(distance.sqrMagnitude) * colliderPosition;
            }    
        }
        

        Vector3 aggregate = cohesionSum + avoidanceSum;
        float remainder = swarmParameters.beeSpeedCap - aggregate.magnitude;
        var buzzinessVector = remainder < 0 ? Vector3.zero : vectorGenerator.GenerateOfMagnitude(remainder);
        
        return (aggregate + buzzinessVector).normalized;

    }

    private float GetCohesionWeight(float distance)
    {
        return Mathf.InverseLerp(swarmParameters.cohesionMinRange,
            swarmParameters.cohesionMaxRange, distance);
    }

    private float GetAvoidanceWeight(float distance)
    {
        var min = swarmParameters.avoidanceMinRange;
        var max = swarmParameters.avoidanceMaxRange;
        return Mathf.Lerp(MathU.Square(max), MathU.Square(min), distance);
       
    }

}
