using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;


public class StillStrategy : ISteeringBehavior
{
    // reference
    private SwarmParameters swarmParameters;
    private Collider myCollider;
    private Transform rootTransform;
    // internals

    private Timer waitTimer;
    private Timer moveTimer;

    private float movesInOneDirectionFor;
    private LayerMask detectionMask1;
    private LayerMask detectionMask2;
    // owned helper objects
    private TimedRandomVectorGenerator vectorGenerator;
    // debug variables
    public Vector3 previousCohesionVector;
    public Vector3 previousAvoidanceVector;

    public void Update(SteeringOutput outputToFill)
    {

    }

    private bool waiting;
    public StillStrategy(Transform root)
    {
        rootTransform = root;
        swarmParameters = SwarmParameters.Instance;
        myCollider = root.gameObject.GetComponent<CapsuleCollider>();
        Debug.Assert(myCollider != null);
        // get these values from swarmParameters
        waitTimer = new Timer(swarmParameters.waitTime);
        moveTimer = new Timer(swarmParameters.moveTime);
        vectorGenerator = new TimedRandomVectorGenerator(swarmParameters.moveTime);

        detectionMask1 = LayerMask.GetMask("Bee");
        detectionMask2 = LayerMask.GetMask("FlyZoneBoundaries");
    }

    public Vector3 CalculateVelocityVector()
    {
        // fix this by adding determineMoving function
        var isWaiting = DetermineIsWaiting();
        return isWaiting ? CalculateWaitingVelocity()
                           : CalculateMovingVelocity();
    }

    public void DebugBehavior()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rootTransform.position, rootTransform.position + previousCohesionVector);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rootTransform.position, rootTransform.position + previousAvoidanceVector);
        
    }

    public Quaternion CalculateRotationQuaternion()
    {
        return Quaternion.LookRotation(rootTransform.forward);
    }

    public bool IsCompleted()
    {
        return false;
    }



    private bool DetermineIsWaiting()
    {
        
        if (waiting)
        {
            waitTimer.TickTimer();
            waiting = ! waitTimer.Check();
            if ( !waiting )
            {
                moveTimer.Reset();
            }
        }
        else 
        {
            moveTimer.TickTimer();
            waiting = moveTimer.Check();
            if ( waiting )
            {
                waitTimer.Reset();
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
        // this function needs to be cleaned up
        var radius = swarmParameters.detectionRadius;
        var detectedBoundaries = Physics.OverlapSphere(rootTransform.position,
                                       radius,
                                        layerMask: detectionMask2);
        // if we run into performance issues, bee detection can be done less frequently and cached instead
        var detectedBees = Physics.OverlapSphere(rootTransform.position,
                                                swarmParameters.avoidanceMaxRange,
                                                layerMask: detectionMask1);
        
        var detectedNeighbors = Physics.OverlapSphere(rootTransform.position,
                                                radius,
                                                layerMask: detectionMask1);

        var closestX = detectedNeighbors
                        .OrderBy(hit => Vector3.SqrMagnitude(hit.transform.position - rootTransform.position))
                        .Take(swarmParameters.detectNumForCohesion)
                        .ToArray();
        // let's see if bees wander to far with this
        // might have to have some logic to detect bees that are further away

        Vector3 cohesionSum = Vector3.zero;
        Vector3 avoidanceSum = Vector3.zero;
        

        foreach (var collider in closestX)
        {
            if (collider == myCollider) continue;
            Vector3 colliderPosition = collider.ClosestPoint(rootTransform.position);
            var distance = colliderPosition - rootTransform.position;
            cohesionSum += GetCohesionWeight(distance.magnitude) * distance.normalized;
        }

        foreach(var collider in detectedBees)
        {
            if (collider == myCollider)
            {
                continue;
            }

            Vector3 colliderPosition = collider.ClosestPoint(rootTransform.position);
            var distance = colliderPosition - rootTransform.position;
            avoidanceSum -= GetAvoidanceWeight(distance.magnitude) * distance.normalized;

        }

        foreach (var collider in detectedBoundaries)
        {
            Vector3 colliderPosition = collider.ClosestPoint(rootTransform.position);
            var distance = colliderPosition - rootTransform.position;
            avoidanceSum -= GetAvoidanceWeight(distance.magnitude) * distance.normalized;
        }

        previousAvoidanceVector = avoidanceSum;
        previousCohesionVector = cohesionSum;
        Vector3 aggregate = cohesionSum + avoidanceSum;
        float remainder = swarmParameters.beeSpeedCap - aggregate.magnitude;
        var buzzinessVector = remainder < 0 ? Vector3.zero : vectorGenerator.GenerateOfMagnitude(remainder);
        
        return (aggregate + buzzinessVector).normalized;

    }

    private float GetCohesionWeight(float distanceSqr)
    {
        return Mathf.InverseLerp(swarmParameters.cohesionMinRange,
            swarmParameters.cohesionMaxRange, distanceSqr);
    }

    private float GetAvoidanceWeight(float distanceSqr)
    {
        var min = swarmParameters.avoidanceMinRange;
        var max = swarmParameters.avoidanceMaxRange;
        return Mathf.InverseLerp(max,min, distanceSqr);
       
    }

}
